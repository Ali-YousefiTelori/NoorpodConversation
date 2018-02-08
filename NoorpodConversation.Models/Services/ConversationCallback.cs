using NoorpodConversation.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoorpodConversation.Models;
using SignalGo.Shared.DataTypes;

namespace NoorpodConversation.Services
{
    [ServiceContract("ConversationCallback")]
    public class ConversationCallback : IConversationCallback
    {
        public void ChangedPermission(UserClient user)
        {
            NoorpodServiceHelper.OnUserChangedPermissionAction?.Invoke(user);
        }

        public void ChangedSpeeckQaulity(int bufferSend, int sampleRate)
        {
            NoorpodServiceHelper.OnChangedSpeeckQaulityAction?.Invoke(bufferSend, sampleRate);
        }

        public void ChangeRoomSubject(string subject)
        {
            NoorpodServiceHelper.OnChangeRoomSubjectAction?.Invoke(subject);
        }

        public void ChangeUserToAdmin(UserClient user)
        {
            NoorpodServiceHelper.OnChangedUserToAdminAction?.Invoke(user);
        }

        public void ClosedRoom()
        {
            NoorpodServiceHelper.IsRoomOpenned = false;
            NoorpodServiceHelper.OnRoomClosedAction?.Invoke();
        }

        public void DeletedMessage(int messageId)
        {
            NoorpodServiceHelper.OnDeletedMessageAction?.Invoke(messageId);
        }

        public void DisconnectedClient(UserClient user)
        {
            NoorpodServiceHelper.OnUserDisconnectedAction?.Invoke(user);
        }

        public void HandDown(UserClient user)
        {
            NoorpodServiceHelper.OnUserHandDownAction?.Invoke(user);
        }

        public void HandUp(UserClient user)
        {
            NoorpodServiceHelper.OnUserHandUpAction?.Invoke(user);
        }

        public void LogginedClient(UserClient user)
        {
            NoorpodServiceHelper.OnUserLogginedAction?.Invoke(user);
        }

        public void OpennedRoom()
        {
            NoorpodServiceHelper.IsRoomOpenned = true;
            NoorpodServiceHelper.OnRoomOpenAction?.Invoke();
        }

        public void ReceivedMessage(MessageInfo message)
        {
            NoorpodServiceHelper.ReceivedMessageAction?.Invoke(message);
        }

        public void SetDefaultUserMicrophone(UserClient userClient)
        {
            NoorpodServiceHelper.OnSetDefaultUserMicrophoneAction?.Invoke(userClient);
        }
    }
}
