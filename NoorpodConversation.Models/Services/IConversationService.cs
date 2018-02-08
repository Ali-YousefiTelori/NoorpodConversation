using NoorpodConversation.Models;
using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.Services
{
    [ServiceContract("ConversationService")]
    public interface IConversationService
    {
        MessageContract<Guid> Register(string userName, string password);

        MessageContract<(UserClient, List<UserClient>)> Login(string userName, string password);

        MessageContract SendMessage(MessageInfo message);

        MessageContract DeleteMessage(int messageId);

        MessageContract ChangeAdminPermission(UserClient client, bool isAdmin);

        MessageContract AllowPermission(UserClient userClient, PermissionEnum permission);

        MessageContract DenyPermission(UserClient userClient, PermissionEnum permission);

        MessageContract HandUp();

        MessageContract HandDown();

        MessageContract SetDefaultUserMicrophone(UserClient userClient);

        MessageContract SetUserMicrophoneOff(UserClient userClient);

        MessageContract<UserClient> GetCurrentUserMicrophone();

        MessageContract CanGetMicrophone(UserClient user);

        MessageContract<List<UserClient>> GetHandUpUsers();

        MessageContract CloseRoom();

        MessageContract OpenRoom();

        MessageContract<bool> IsRoomOpen();

        MessageContract<string> GetLastRoomMessage();

        MessageContract AddRoomMessage(string message);

        MessageContract ChangeRoomSubject(string subject);

        MessageContract<string> GetRoomSubject();

        MessageContract GetMessageForLastApplicationVersion(int versionNumber);

        MessageContract ChangeSpeeckQaulity(int bufferSend, int sampleRate);

        MessageContract<(int, int)> GetLastSpeeckQaulity();
    }
}
