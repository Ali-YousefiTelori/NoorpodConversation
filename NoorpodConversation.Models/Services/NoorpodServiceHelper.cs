using NoorpodConversation.Helper;
using NoorpodConversation.Models;
using SignalGo.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoorpodConversation.Services
{
    public static class NoorpodServiceHelper
    {
        public static bool ApplicationClosed = false;
        static bool _IsConnected;
        public static bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                _IsConnected = value;
            }
        }

        public static bool IsSendAudioNow { get; set; }
        public static bool IsRoomOpenned { get; set; }
        public static UserClient CurrentUser { get; set; }
        public static Action ReLoginAction { get; set; }
        public static Action<UserClient> OnUserLogginedAction { get; set; }
        public static Action<UserClient> OnUserDisconnectedAction { get; set; }
        public static Action<UserClient> OnUserChangedPermissionAction { get; set; }
        public static Action<int> OnDeletedMessageAction { get; set; }
        public static Action<UserClient> OnChangedUserToAdminAction { get; set; }
        public static Action<UserClient> OnUserHandUpAction { get; set; }
        public static Action<UserClient> OnUserHandDownAction { get; set; }
        public static Action<UserClient> OnSetDefaultUserMicrophoneAction { get; set; }
        public static Action OnRoomClosedAction { get; set; }
        public static Action<string> OnApplicationCloseAction { get; set; }
        public static Action OnRoomOpenAction { get; set; }
        public static Action<string> OnChangeRoomSubjectAction { get; set; }

        public static ClientProvider connection = null;
        static IConversationService currentService;
        //static TcpClient _TcpClient = null;
        public static Action<MessageInfo> ReceivedMessageAction { get; set; }
        static object threadSafe = new object();
        public static void Initialize()
        {
            lock (threadSafe)
            {
                if (ApplicationClosed)
                    return;
                if (IsConnected)
                    return;
                Disconnect();
                System.Net.ServicePointManager.Expect100Continue = false;
                string url = @"http://localhost:6987/Noorpods/ConversationService";
                connection = new ClientProvider();
                connection.Connect(url);
                IPHostEntry Host = Dns.GetHostEntry("localhost");
                connection.ConnectToUDP(Host.AddressList.FirstOrDefault().ToString(), 13020);
                connection.OnReceivedData = (bytes) =>
                {
                    if (ApplicationClosed || !IsConnected)
                        return;
                    AudioReceivedFunction?.Invoke(bytes);
                };
                var callbacks = connection.RegisterServerCallback<ConversationCallback>();
                SignalGo.Shared.Helpers.CSCodeInjection.OnErrorAction = (ss, types, source) =>
                {

                };
                currentService = connection.RegisterClientServiceInterfaceWrapper<IConversationService>();

                var versionMSG = GetMessageForLastApplicationVersion(3);
                if (!versionMSG.IsSuccess)
                {
                    OnApplicationCloseAction?.Invoke(versionMSG.Message);
                    return;
                }
                connection.OnConnectionChanged = (value) =>
                {
                    if (value)
                        return;
                    if (ApplicationClosed)
                        return;
                    IsConnected = false;
                    ReLoginAction?.Invoke();
                };
                //connection.StateChanged += Connection_StateChanged;
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.

                IsConnected = true;
            }
        }


        public static MessageContract<Guid> Register(string userName, string password)
        {
            return currentService.Register(userName,password);
        }

        public static MessageContract<(UserClient, List<UserClient>)> Login(string userName, string password)
        {
            return currentService.Login(userName, password);
        }

        public static MessageContract SendMessage(MessageInfo message)
        {
            return currentService.SendMessage(message);
        }

        public static MessageContract AllowPermission(UserClient userClient, PermissionEnum permission)
        {
            return currentService.AllowPermission(userClient, permission);
        }

        public static MessageContract DenyPermission(UserClient userClient, PermissionEnum permission)
        {
            return currentService.DenyPermission(userClient, permission);
        }

        public static MessageContract HandUp()
        {
            return currentService.HandUp();
        }

        public static MessageContract HandDown()
        {
            return currentService.HandDown();
        }

        public static MessageContract DeleteMessage(int messageId)
        {
            return currentService.DeleteMessage(messageId);
        }

        public static MessageContract ChangeAdminPermission(UserClient client, bool isAdmin)
        {
            return currentService.ChangeAdminPermission(client, isAdmin);
        }

        public static MessageContract SetDefaultUserMicrophone(UserClient user)
        {
            return currentService.SetDefaultUserMicrophone(user);
        }

        public static MessageContract SetUserMicrophoneOff(UserClient userClient)
        {
            return currentService.SetUserMicrophoneOff(userClient);
        }

        public static MessageContract CanGetMicrophone(UserClient user)
        {
            return currentService.CanGetMicrophone(user);
        }

        public static MessageContract CloseRoom()
        {
            return currentService.CloseRoom();
        }

        public static MessageContract OpenRoom()
        {
            return currentService.OpenRoom();
        }

        public static MessageContract<bool> IsRoomOpen()
        {
            return currentService.IsRoomOpen();
        }

        public static MessageContract<string> GetLastRoomMessage()
        {
            return currentService.GetLastRoomMessage();
        }

        public static MessageContract AddRoomMessage(string message)
        {
            return currentService.AddRoomMessage(message);
        }

        public static MessageContract ChangeRoomSubject(string subject)
        {
            return currentService.ChangeRoomSubject(subject);
        }

        public static MessageContract<string> GetRoomSubject()
        {
            return currentService.GetRoomSubject();
        }

        public static MessageContract GetMessageForLastApplicationVersion(int versionNumber)
        {
            return currentService.GetMessageForLastApplicationVersion(versionNumber);
        }

        public static MessageContract<UserClient> GetCurrentUserMicrophone()
        {
            return currentService.GetCurrentUserMicrophone();
        }

        public static MessageContract ChangeSpeeckQaulity(int bufferSend, int sampleRate)
        {
            return currentService.ChangeSpeeckQaulity(bufferSend, sampleRate);
        }

        public static MessageContract<(int, int)> GetLastSpeeckQaulity()
        {
            return currentService.GetLastSpeeckQaulity();
        }
        
        //public static void StartSocket()
        //{
        //    Task task = new Task(() =>
        //    {
        //        try
        //        {
        //            // Translate the passed message into ASCII and store it as a Byte array.

        //            // Get a client stream for reading and writing.
        //            //  Stream stream = client.GetStream();
        //            var client = _TcpClient;
        //            NetworkStream stream = client.GetStream();

        //            // Buffer to store the response bytes.

        //            // Read the first batch of the TcpServer response bytes.
        //            while (true)
        //            {
        //                if (ApplicationClosed || !IsConnected)
        //                    break;
        //                var data = SocketMessageHelper.ReadMessage(stream);
        //                if (data == null)
        //                {
        //                    Thread.Sleep(1000);
        //                    continue;
        //                }
        //                try
        //                {
        //                    AudioReceivedFunction?.Invoke(data);
        //                }
        //                catch (Exception ex)
        //                {
        //                    AutoLogger.LogError(ex, "AudioReceivedFunction");
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            if (ApplicationClosed)
        //                return;
        //            IsConnected = false;
        //            Disconnect();
        //            ReLoginAction.Invoke();
        //        }
        //    });
        //    task.Start();
        //}

        public static void Disconnect()
        {
            try
            {
                if (connection != null && !connection.IsDisposed)
                    connection.Dispose();
            }
            catch
            {

            }

            //try
            //{
            //    _TcpClient.Close();
            //}
            //catch
            //{

            //}
        }

        public static Action<byte[]> AudioReceivedFunction { get; set; }
        public static Action<int, int> OnChangedSpeeckQaulityAction { get; set; }

        public static void SendAudio(byte[] bytes)
        {
            //var data = SocketMessageHelper.CreateMesssage(bytes);
            connection.SendUdpData(bytes);

            //_NoorpodServiceHub.Invoke("SendSoundData", bytes).Wait();
        }
    }
}
