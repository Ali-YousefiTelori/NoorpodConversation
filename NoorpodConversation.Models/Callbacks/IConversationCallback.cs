using NoorpodConversation.Models;
using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.Callbacks
{
    [ServiceContract("ConversationCallback")]
    public interface IConversationCallback
    {
        void DisconnectedClient(UserClient user);
        void LogginedClient(UserClient currentUser);
        void ReceivedMessage(MessageInfo message);
        void DeletedMessage(int messageId);
        void ChangeUserToAdmin(UserClient client);
        void ChangedPermission(UserClient find);
        void HandUp(UserClient currentUser);
        void HandDown(UserClient currentUser);
        void SetDefaultUserMicrophone(UserClient userClient);
        void ClosedRoom();
        void OpennedRoom();
        void ChangeRoomSubject(string subject);
        void ChangedSpeeckQaulity(int bufferSend, int sampleRate);
    }
}
