using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.Models
{
    public class MessageInfo
    {
        public int Id { get; set; }
        public UserClient Sender { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
        [JsonIgnore()]
        public bool IsCurrentUserMessage { get; set; }
        [JsonIgnore()]
        public bool IsLoginMessage { get; set; }
        [JsonIgnore()]
        public bool IsLogoutMessage { get; set; }
    }
}
