using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NoorpodConversation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoorpodConversation.Web.Hubs
{
    [HubName("AudioConversationHub")]
    public class AudioConversationHub : Hub
    {
        public MessageContract Login(string userName, Guid userHash)
        {
            return this.RunAction(() =>
            {
                return MessageContract.Success();
            });
        }

        public void SendSoundData(byte[] bytes)
        {
            Clients.AllExcept(this.Context.ConnectionId).ReceivedSoundData(bytes);
        }

        public MessageContract GetLastError()
        {
            return this.RunAction(() =>
            {
                return MvcApplication.LastError.Success();
            });
        }
    }
}