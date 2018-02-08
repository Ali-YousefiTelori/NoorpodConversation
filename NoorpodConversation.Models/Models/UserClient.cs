using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NoorpodConversation.Models
{
    public enum PermissionEnum
    {
        Microphone = 0,
        Message = 1,
        Login = 2
    }

    public class UserClient : INotifyPropertyChanged
    {
        public UserClient()
        {
            NoPermissions = new List<PermissionEnum>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public List<PermissionEnum> NoPermissions { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime HandDateTime { get; set; }
        public bool IsTalking { get; set; }
        public string SessionId { get; set; }

        bool _IsHandUp;
        [JsonIgnore()]
        public bool IsHandUp
        {
            get
            {
                return _IsHandUp;
            }
            set
            {
                _IsHandUp = value;
                OnPropertyChanged("IsHandUp");
            }
        }

        [JsonIgnore()]
        public bool IsBanMic
        {
            get
            {
                return NoPermissions.Contains(PermissionEnum.Microphone);
            }
        }

        [JsonIgnore()]
        public bool IsBanMail
        {
            get
            {
                return NoPermissions.Contains(PermissionEnum.Message);
            }
        }
        

        public void RefreshIcons()
        {
            OnPropertyChanged("IsBanMail");
            OnPropertyChanged("IsBanMic");
            OnPropertyChanged("IsHandUp");
            OnPropertyChanged("IsAdmin");
            OnPropertyChanged("IsTalking");
        }


        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
