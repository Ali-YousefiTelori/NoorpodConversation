using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.BaseViewModels.Models
{
    public class MessageBoxBusy : ANotifyPropertyChanged
    {
        public static MessageBoxBusy Current { get; set; }

        public Action OkCommandAction { get; set; }
        public Action CancelCommandAction { get; set; }

        string _TitleMessage;

        public string TitleMessage
        {
            get { return _TitleMessage; }
            set { _TitleMessage = value; OnPropertyChanged("TitleMessage"); }
        }

        string _contentMessage;

        public string ContentMessage
        {
            get { return _contentMessage; }
            set { _contentMessage = value; OnPropertyChanged("ContentMessage"); }
        }

        bool _IsBusy = false;

        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; OnPropertyChanged("IsBusy"); }
        }
    }
}
