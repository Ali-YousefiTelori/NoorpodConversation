using NoorpodConversation.BaseViewModels.Collections;
using NoorpodConversation.Models;
using NoorpodConversation.Services;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.BaseViewModels.Views
{
    public class MessengerPageBaseViewModel : ANotifyPropertyChanged
    {
        public static Action ScrollToDownNowAction { get; set; }
        public static MessengerPageBaseViewModel This { get; set; }
        public MessengerPageBaseViewModel()
        {
            This = this;
            //Users.Add(new UserClient() { UserName = "ali", IsHandUp = true, NoPermissions = new List<NoorpodConversation.Models.PermissionEnum>() { PermissionEnum.Message } });
            //Users.Add(new UserClient() { UserName = "علی ایرانی", IsAdmin = true });
            //Users.Add(new UserClient() { UserName = "علی یوسفی", NoPermissions = new List<NoorpodConversation.Models.PermissionEnum>() { PermissionEnum.Microphone } });
            //Messages.Add(new MessageInfo() { Message = "test", IsCurrentUserMessage = true, Sender= Users[1] });
            //Messages.Add(new MessageInfo() { Message = " s sd ds sd sd sd sd sd ", Sender=Users.First() });
            //Messages.Add(new MessageInfo() { Message = "این یک پیغام تست می باشد", Sender = Users.First() });
            //Messages.Add(new MessageInfo() { Message = "سلام چطوری", IsCurrentUserMessage = true, Sender = Users[1] });
            //Messages.Add(new MessageInfo() { Message = "سلام چه خبر؟", Sender = Users.Last() });
            //Messages.Add(new MessageInfo() { Message = "این یک پیغام طولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشدطولانی تست می باشد", Sender = Users.Last() });
            NoorpodServiceHelper.ReceivedMessageAction = (msg) =>
            {
                msg.IsCurrentUserMessage = msg.Sender.UserName == NoorpodServiceHelper.CurrentUser.UserName;
                if (!msg.IsCurrentUserMessage)
                {
                    ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                    {
                        InformingMessage.InstanceLoading(msg.Sender.UserName, msg.Message);
                    });
                }
                Messages.Add(msg);
                ScrollToDownNowAction?.Invoke();
            };

            NoorpodServiceHelper.OnUserLogginedAction = (user) =>
            {
                var find = (from x in Users where x.UserName == user.UserName select x).FirstOrDefault();
                if (find != null)
                    Users.Remove(find);
                Users.Add(user);
                Messages.Add(new MessageInfo() { IsLoginMessage = true, Message = " وارد اتاق شد.", Sender = user });
            };
            
            NoorpodServiceHelper.OnUserDisconnectedAction = (user) =>
            {
                Users.Remove((from x in Users where x.SessionId == user.SessionId select x).FirstOrDefault());
                Messages.Add(new MessageInfo() { IsLogoutMessage = true, Message = " از اتاق خارج شد.", Sender = user });
            };

            NoorpodServiceHelper.OnDeletedMessageAction = (messageId) =>
            {
                Messages.RemoveRange((from x in Messages where x.Id == messageId select x));
            };

            NoorpodServiceHelper.OnUserChangedPermissionAction = (user) =>
            {
                foreach (var item in (from x in Users where x.UserName == user.UserName select x))
                {
                    item.NoPermissions = user.NoPermissions;
                    item.RefreshIcons();
                }
                if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                {
                    NoorpodServiceHelper.CurrentUser.NoPermissions = user.NoPermissions;
                    if (user.NoPermissions.Contains(PermissionEnum.Login))
                        NoorpodServiceHelper.Disconnect();
                    NoorpodServiceHelper.CurrentUser.RefreshIcons();
                    ChangedPermissions();
                }
            };

            NoorpodServiceHelper.OnChangedUserToAdminAction = (user) =>
            {
                foreach (var item in (from x in Users where x.UserName == user.UserName select x))
                {
                    item.IsAdmin = user.IsAdmin;
                    item.NoPermissions = user.NoPermissions;
                    item.RefreshIcons();
                }
                if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                {
                    NoorpodServiceHelper.CurrentUser.IsAdmin = user.IsAdmin;
                    NoorpodServiceHelper.CurrentUser.NoPermissions = user.NoPermissions;
                    NoorpodServiceHelper.CurrentUser.RefreshIcons();
                    ChangedPermissions();
                }
            };

            NoorpodServiceHelper.OnUserHandUpAction = (user) =>
            {
                foreach (var item in (from x in Users where x.UserName == user.UserName select x))
                {
                    item.HandDateTime = user.HandDateTime;
                    item.IsHandUp = true;
                    item.RefreshIcons();
                }
                if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                {
                    NoorpodServiceHelper.CurrentUser.HandDateTime = user.HandDateTime;
                    NoorpodServiceHelper.CurrentUser.IsHandUp = true;
                    NoorpodServiceHelper.CurrentUser.RefreshIcons();
                    ChangedPermissions();
                    canHandUp = true;
                }
                Users.SortByAscending(x => x.HandDateTime);
                Users.SortBy(x => x.IsHandUp);
                Users.SortBy(x => x.IsTalking);
                ApplicationHelper.Current.RefreshCommandsAction();
            };

            NoorpodServiceHelper.OnUserHandDownAction = (user) =>
            {
                foreach (var item in (from x in Users where x.UserName == user.UserName select x))
                {
                    item.HandDateTime = user.HandDateTime;
                    item.IsHandUp = false;
                    item.RefreshIcons();
                }
                if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                {
                    NoorpodServiceHelper.CurrentUser.HandDateTime = user.HandDateTime;
                    NoorpodServiceHelper.CurrentUser.IsHandUp = false;
                    NoorpodServiceHelper.CurrentUser.RefreshIcons();
                    ChangedPermissions();
                    canHandUp = true;
                }
                Users.SortByAscending(x => x.HandDateTime);
                Users.SortBy(x => x.IsHandUp);
                Users.SortBy(x => x.IsTalking);
                ApplicationHelper.Current.RefreshCommandsAction();
            };

            NoorpodServiceHelper.OnSetDefaultUserMicrophoneAction = (user) =>
            {
                foreach (var item in Users)
                {
                    item.IsTalking = false;
                }

                foreach (var item in (from x in Users where x.UserName == user.UserName select x))
                {
                    item.IsTalking = user.IsTalking;
                    item.RefreshIcons();
                }

                if (user.UserName != NoorpodServiceHelper.CurrentUser.UserName)
                {
                    NoorpodServiceHelper.CurrentUser.IsTalking = false;
                    NoorpodServiceHelper.IsSendAudioNow = false;
                }
                else
                {
                    NoorpodServiceHelper.CurrentUser.IsTalking = user.IsTalking;
                }
                NoorpodServiceHelper.CurrentUser.RefreshIcons();
                ChangedPermissions();
                ApplicationHelper.Current.RefreshCommandsAction();
                Users.SortByAscending(x => x.HandDateTime);
                Users.SortBy(x => x.IsHandUp);
                Users.SortBy(x => x.IsTalking);
            };

            NoorpodServiceHelper.OnChangeRoomSubjectAction = (subject) =>
            {
                RoomSubject = subject;
            };
        }

        public void ChangedPermissions()
        {
            OnPropertyChanged("IsAccessMessage");
            OnPropertyChanged("IsAccessMicrophone");
            OnPropertyChanged("IsAdmin");
            OnPropertyChanged("IsHandUp");
            OnPropertyChanged("IsTalking");
            OnPropertyChanged("IsRoomOpenned");
        }


        string _Message;

        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                OnPropertyChanged("Message");
            }
        }

        private string _roomSubject;
        public string RoomSubject
        {
            get
            {
                return _roomSubject;
            }
            set
            {
                _roomSubject = value;
                OnPropertyChanged("RoomSubject");
            }
        }

        public bool IsTalking
        {
            get
            {
                return NoorpodServiceHelper.IsSendAudioNow;
            }
        }


        public bool IsRoomOpenned
        {
            get
            {
                return NoorpodServiceHelper.IsRoomOpenned;
            }
        }

        public bool IsAccessMessage
        {
            get
            {
                if (NoorpodServiceHelper.CurrentUser == null)
                    return false;
                return (bool)!NoorpodServiceHelper.CurrentUser.NoPermissions.Contains(PermissionEnum.Message);
            }
        }

        public bool IsHandUp
        {
            get
            {
                if (NoorpodServiceHelper.CurrentUser == null)
                    return false;
                return (bool)NoorpodServiceHelper.CurrentUser.IsHandUp;
            }
        }

        public bool IsAccessMicrophone
        {
            get
            {
                if (NoorpodServiceHelper.CurrentUser == null)
                    return false;
                return (bool)!NoorpodServiceHelper.CurrentUser.NoPermissions.Contains(PermissionEnum.Microphone);
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (NoorpodServiceHelper.CurrentUser == null)
                    return false;
                return (bool)NoorpodServiceHelper.CurrentUser.IsAdmin;
            }
        }

        FastCollection<UserClient> _Users = null;
        public FastCollection<UserClient> Users
        {
            get
            {
                if (_Users == null)
                    _Users = new FastCollection<UserClient>(ApplicationHelper.Current == null ? null : ApplicationHelper.Current.DispatcherThread);
                return _Users;
            }
            set
            {
                _Users = value;
            }
        }

        public FastCollection<MessageInfo> Messages
        {
            get
            {
                if (_Messages == null)
                    _Messages = new FastCollection<MessageInfo>(ApplicationHelper.Current == null ? null : ApplicationHelper.Current.DispatcherThread);
                return _Messages;
            }
            set
            {
                _Messages = value;
            }
        }


        FastCollection<MessageInfo> _Messages = null;

        public void SendMessage()
        {
            string msg = Message;
            Message = "";
            AsyncRunner.Run(() =>
            {
                try
                {
                   var dd= NoorpodServiceHelper.SendMessage(new MessageInfo() { Message = msg ,Sender = NoorpodServiceHelper.CurrentUser});
                }
                catch (Exception ex)
                {
                    Message = msg;
                    AutoLogger.LogError(ex, "SendMessage");
                }
            });
        }

        public bool CanSendMessage()
        {
            return !string.IsNullOrEmpty(Message) && IsAccessMessage;
        }

        public void DeleteMessage(MessageInfo msg)
        {
            if (msg.IsLoginMessage || msg.IsLogoutMessage)
                return;
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.DeleteMessage(msg.Id);
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "DeleteMessage");
                }
            });
        }


        public void ChangeUserAdmin(UserClient user)
        {
            if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                return;
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.ChangeAdminPermission(user, !user.IsAdmin);
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "ChangeUserAdmin");
                }
            });
        }

        public void DenyMicrophonePermission(UserClient user)
        {
            if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                return;
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.DenyPermission(user, PermissionEnum.Microphone);
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "DenyLoginPermission");
                }
            });
        }

        public void DenyMessagePermission(UserClient user)
        {
            if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                return;
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.DenyPermission(user, PermissionEnum.Message);
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "DenyLoginPermission");
                }
            });
        }

        public void AllowLoginPermission(UserClient user)
        {
            if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                return;
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.AllowPermission(user, PermissionEnum.Login);
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "DenyLoginPermission");
                }
            });
        }

        public void AllowMicrophonePermission(UserClient user)
        {
            if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                return;
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.AllowPermission(user, PermissionEnum.Microphone);
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "DenyLoginPermission");
                }
            });
        }

        public void AllowMessagePermission(UserClient user)
        {
            if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                return;
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.AllowPermission(user, PermissionEnum.Message);
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "DenyLoginPermission");
                }
            });
        }

        bool canHandUp = true;
        public void HandUp()
        {
            AsyncRunner.Run(() =>
            {
                try
                {
                    canHandUp = false;
                    MessageContract data = null;
                    if (IsHandUp)
                        data = NoorpodServiceHelper.HandDown();
                    else
                        data = NoorpodServiceHelper.HandUp();
                    if (!data.IsSuccess)
                        canHandUp = true;
                    ApplicationHelper.Current.RefreshCommandsAction();
                }
                catch (Exception ex)
                {
                    canHandUp = true;
                    AutoLogger.LogError(ex, "HandUp");
                }
            });
        }


        public bool CanHandUp()
        {
            return canHandUp;
        }

        public void SetDefaultMicrophone(UserClient user)
        {
            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.SetDefaultUserMicrophone(user);
                }
                catch (Exception ex)
                {
                    canHandUp = true;
                    AutoLogger.LogError(ex, "HandUp");
                }
            });
        }
    }
}
