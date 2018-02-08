using NoorpodConversation.Callbacks;
using NoorpodConversation.DataBase.Context;
using NoorpodConversation.DataBase.Models;
using NoorpodConversation.Models;
using SignalGo.Server.Models;
using SignalGo.Shared;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NoorpodConversation.Services
{
    /// <summary>
    /// LogginedClient
    /// DisconnectedClient
    /// ReceivedMessage
    /// HandUp
    /// HandDown
    /// DeletedMessage
    /// </summary>
    [ServiceContract("ConversationService")]
    public class ConversationService
    {
        UserClient CurrentUser { get; set; }
        public string CurrentSesssion { get; set; }
        public IConversationCallback callback = null;
        OperationContext CurrentContext { get; set; }
        public ConversationService()
        {
            try
            {
                AutoLogger.LogText("on connected");
                CurrentContext = OperationContext.Current;
                callback = CurrentContext.GetClientCallback<IConversationCallback>();
                CurrentSesssion = CurrentContext.Client.ClientId;
                CurrentContext.Client.OnDisconnected = () =>
                {
                    try
                    {
                        AutoLogger.LogText("on disconnected");
                        clientSessions.Remove(CurrentSesssion);
                        foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                        {
                            AsyncActions.Run(() => { item.DisconnectedClient(CurrentUser); });
                        }
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "ConversationService OnDisconnected");
                    }
                };
            }
            catch (Exception ex)
            {
                AutoLogger.LogError(ex, "ConversationService constractor");
            }
        }


        static Dictionary<string, UserClient> clientSessions = new Dictionary<string, UserClient>();
        static List<UserClient> HandUpClients = new List<UserClient>();

        void RegisterSession(UserClient user)
        {
            if (!clientSessions.ContainsKey(CurrentSesssion))
            {
                CurrentUser = user;
                user.SessionId = CurrentSesssion;
                clientSessions.Add(CurrentSesssion, user);
            }
        }

        public MessageContract Register(string userName, string password)
        {
            return this.RunAction(() =>
            {
                using (NoorpodContext context = new NoorpodContext())
                {
                    var find = (from x in context.Users where x.UserName == userName select x).FirstOrDefault();
                    if (find != null)
                        return MessageContract.Fail("Exist");
                    else
                    {
                        UserInfo user = new UserInfo() { UserName = userName, IsAdmin = false, Password = password };
                        context.Users.Add(user);
                        context.SaveChanges();
                        return MessageContract.Success();
                    }
                }
            });
        }

        public MessageContract<(UserClient, List<UserClient>)> Login(string userName, string password)
        {
            return this.RunAction(() =>
            {
                using (NoorpodContext context = new NoorpodContext())
                {
                    Guid.TryParse(password, out Guid userHash);
                    var find = (from x in context.Users where x.UserName == userName && x.Password == password select x).FirstOrDefault();
                    if (find == null)
                        return MessageContract<(UserClient, List<UserClient>)>.Fail("not exist");
                    if (!find.IsAdmin && !IsRoomOpened)
                        return MessageContract<(UserClient, List<UserClient>)>.Fail("room closed");

                    var client = new UserClient() { UserName = find.UserName, IsAdmin = find.IsAdmin };
                    var permissions = (from x in context.NotPermissions where x.UserId == find.Id select x.PermissionType).ToList();
                    if (permissions.Contains(PermissionEnum.Login))
                        return MessageContract<(UserClient, List<UserClient>)>.Fail("access denied");
                    client.NoPermissions.AddRange(permissions);
                    RegisterSession(client);
                    foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                    {
                        AsyncActions.Run(() => { item.LogginedClient(CurrentUser); });
                    }

                    UserLoginCheckHand();
                    //بازگشت دادن اطلاعات کاربر و کاربرانی که انلاین هستند
                    return (CurrentUser, clientSessions.Values.ToList()).Success();
                }
            });
        }

        static int MessageId = 1;
        static object lockOBJ = new object();
        public MessageContract SendMessage(MessageInfo message)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                lock (lockOBJ)
                {
                    if (CurrentUser.NoPermissions.Contains(PermissionEnum.Message))
                        return MessageContract.Fail("no access");
                    if (!CurrentUser.IsAdmin && !IsRoomOpened)
                        return MessageContract.Fail("room closed");
                    message.CreatedDateTime = DateTime.Now;
                    message.Id = MessageId;

                    foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                    {
                        AsyncActions.Run(() => { item.ReceivedMessage(message); });
                    }
                    AutoLogger.LogText("MSG" + message.Sender.UserName + " : " + message.Message + " " + message.CreatedDateTime);
                    MessageId++;
                    return MessageContract.Success();
                }
            });
        }

        public MessageContract DeleteMessage(int messageId)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                lock (lockOBJ)
                {
                    if (!CurrentUser.IsAdmin)
                        return MessageContract.Fail("no access");
                    foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                    {
                        AsyncActions.Run(() => { item.DeletedMessage(messageId); });
                    }
                    return MessageContract.Success();
                }
            });
        }

        public MessageContract ChangeAdminPermission(UserClient client, bool isAdmin)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                using (NoorpodContext context = new NoorpodContext())
                {
                    var sessionUser = (from x in clientSessions where x.Value.UserName == client.UserName select x.Value).FirstOrDefault();

                    var find = (from x in context.Users where x.UserName == client.UserName select x).FirstOrDefault();
                    if (find == null)
                        return MessageContract.Fail("user not found");
                    find.IsAdmin = isAdmin;
                    client.IsAdmin = isAdmin;
                    if (sessionUser != null)
                        sessionUser.IsAdmin = isAdmin;
                    context.SaveChanges();
                    foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                    {
                        AsyncActions.Run(() => { item.ChangeUserToAdmin(client); });
                    }
                    return MessageContract.Success();
                }
            });
        }

        public MessageContract AllowPermission(UserClient userClient, PermissionEnum permission)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                var find = (from x in clientSessions where x.Value.UserName == userClient.UserName select x.Value).FirstOrDefault();
                using (NoorpodContext context = new NoorpodContext())
                {
                    var user = (from x in context.Users where x.UserName == find.UserName select x).FirstOrDefault();
                    var finper = (from x in context.NotPermissions where x.UserId == user.Id && x.PermissionType == permission select x).FirstOrDefault();
                    if (finper != null)
                    {
                        context.NotPermissions.Remove(finper);
                        context.SaveChanges();
                    }
                }
                if (find.NoPermissions.Contains(permission))
                    find.NoPermissions.Remove(permission);
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.ChangedPermission(find); });
                }
                return MessageContract.Success();
            });
        }

        public MessageContract DenyPermission(UserClient userClient, PermissionEnum permission)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                var find = (from x in clientSessions where x.Value.UserName == userClient.UserName select x.Value).FirstOrDefault();
                using (NoorpodContext context = new NoorpodContext())
                {
                    var user = (from x in context.Users where x.UserName == find.UserName select x).FirstOrDefault();
                    var finper = (from x in context.NotPermissions where x.UserId == user.Id && x.PermissionType == permission select x).FirstOrDefault();
                    if (finper == null)
                    {
                        context.NotPermissions.Add(new NotPermissionInfo() { PermissionType = permission, UserId = user.Id });
                        context.SaveChanges();
                    }
                }
                if (!find.NoPermissions.Contains(permission))
                    find.NoPermissions.Add(permission);
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.ChangedPermission(find); });
                }
                return MessageContract.Success();
            });
        }

        public MessageContract HandUp()
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!HandUpClients.Contains(CurrentUser))
                    HandUpClients.Add(CurrentUser);
                CurrentUser.HandDateTime = DateTime.Now;
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.HandUp(CurrentUser); });
                }
                return MessageContract.Success();
            });
        }

        void UserLoginCheckHand()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                var find = (from x in HandUpClients where x.UserName == CurrentUser.UserName select x).FirstOrDefault();
                if (find != null)
                {
                    if (find != CurrentUser)
                    {
                        HandUpClients.Remove(find);
                        HandUpClients.Add(CurrentUser);
                        CurrentUser.HandDateTime = find.HandDateTime;
                        foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                        {
                            AsyncActions.Run(() => { item.HandUp(CurrentUser); });
                        }
                    }
                }
            });
        }

        public MessageContract HandDown()
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                HandUpClients.Remove(CurrentUser);
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.HandDown(CurrentUser); });
                }
                return MessageContract.Success();
            });
        }

        static UserClient CurrentUserAccessMicrophone { get; set; }

        public MessageContract SetDefaultUserMicrophone(UserClient userClient)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                //if (!CurrentUser.IsAdmin)
                //    return MessageContract.Fail("no access");
                var find = (from x in clientSessions where x.Value.UserName == userClient.UserName select x.Value).FirstOrDefault();
                if (find.NoPermissions.Contains(PermissionEnum.Microphone))
                    return MessageContract.Fail("no");
                if (CurrentUserAccessMicrophone != null)
                    CurrentUserAccessMicrophone.IsTalking = false;
                CurrentUserAccessMicrophone = find;
                CurrentUserAccessMicrophone.IsTalking = true;
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.SetDefaultUserMicrophone(find); });
                }
                return MessageContract.Success();
            });
        }

        public MessageContract SetUserMicrophoneOff(UserClient userClient)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                userClient.IsTalking = false;
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.SetDefaultUserMicrophone(userClient); });
                }
                return MessageContract.Success();
            });
        }

        public MessageContract<UserClient> GetCurrentUserMicrophone()
        {
            if (CurrentUser == null)
                return MessageContract<UserClient>.Fail("no login");
            return this.RunAction(() =>
            {
                return MessageContract<UserClient>.Success(CurrentUserAccessMicrophone);
            });
        }

        public MessageContract CanGetMicrophone(UserClient user)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (CurrentUserAccessMicrophone != null && CurrentUser.UserName == CurrentUserAccessMicrophone.UserName)
                    return MessageContract.Success();
                return MessageContract.Fail("no");
            });
        }

        public MessageContract<List<UserClient>> GetHandUpUsers()
        {
            if (CurrentUser == null)
                return MessageContract<List<UserClient>>.Fail("no login");
            return this.RunAction(() =>
            {
                return HandUpClients.Success();
            });
        }

        static bool IsRoomOpened { get; set; } = false;

        public MessageContract CloseRoom()
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                IsRoomOpened = false;
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.ClosedRoom(); });
                }
                return MessageContract.Success();
            });
        }

        public MessageContract OpenRoom()
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                IsRoomOpened = true;
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.OpennedRoom(); });
                }
                return MessageContract.Success();
            });
        }

        public MessageContract<bool> IsRoomOpen()
        {
            return this.RunAction(() =>
            {
                return MessageContract<bool>.Success(IsRoomOpened);
            });
        }

        public MessageContract<string> GetLastRoomMessage()
        {
            return this.RunAction(() =>
            {
                using (NoorpodContext context = new NoorpodContext())
                {
                    var item = context.RoomMessages.OrderByDescending(x => x.DateTime).FirstOrDefault();
                    return MessageContract<string>.Success(item == null ? "اتاق بسته است." : item.Message);
                }
            });
        }

        public MessageContract AddRoomMessage(string message)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                using (NoorpodContext context = new NoorpodContext())
                {
                    context.RoomMessages.Add(new RoomMessage() { Message = message, DateTime = DateTime.Now });
                    context.SaveChanges();
                }
                return MessageContract.Success();
            });
        }

        public MessageContract ChangeRoomSubject(string subject)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                using (NoorpodContext context = new NoorpodContext())
                {
                    context.RoomSubjects.Add(new RoomSubject() { Subject = subject, DateTime = DateTime.Now });
                    context.SaveChanges();
                    foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                    {
                        AsyncActions.Run(() => { item.ChangeRoomSubject(subject); });
                    }
                }
                return MessageContract.Success();
            });
        }

        public MessageContract<string> GetRoomSubject()
        {
            if (CurrentUser == null)
                return MessageContract<string>.Fail("no login");
            return this.RunAction(() =>
            {
                using (NoorpodContext context = new NoorpodContext())
                {
                    var item = context.RoomSubjects.OrderByDescending(x => x.DateTime).FirstOrDefault();
                    return MessageContract<string>.Success(item == null ? "همگانی" : item.Subject);
                }
            });
        }

        public MessageContract GetMessageForLastApplicationVersion(int versionNumber)
        {
            return this.RunAction(() =>
            {
                if (versionNumber < 3)
                    return MessageContract.Fail("لطفاً نرم افزار خود را به نسخه ی جدید بروزرسانی کنید،شما در حال استفاده از نسخه ی قدیمی نرم افزار هستید و نیاز دارید تا آن را بروز رسانی کرده و مجدد از آن استفاده کنید.");
                return MessageContract.Success();
            });
        }

        static int _bufferSend = 10;
        static int _sampleRate = 9000;

        public MessageContract ChangeSpeeckQaulity(int bufferSend, int sampleRate)
        {
            if (CurrentUser == null)
                return MessageContract.Fail("no login");
            return this.RunAction(() =>
            {
                if (!CurrentUser.IsAdmin)
                    return MessageContract.Fail("no access");
                _sampleRate = sampleRate;
                _bufferSend = bufferSend;
                foreach (var item in CurrentContext.GetAllClientCallbackList<IConversationCallback>())
                {
                    AsyncActions.Run(() => { item.ChangedSpeeckQaulity(bufferSend, sampleRate); });
                }
                return MessageContract.Success();
            });
        }

        public MessageContract<(int, int)> GetLastSpeeckQaulity()
        {
            if (CurrentUser == null)
                return MessageContract<(int, int)>.Fail("no login");
            return this.RunAction(() =>
            {
                return MessageContract<(int, int)>.Success((_bufferSend, _sampleRate));
            });
        }
    }
}