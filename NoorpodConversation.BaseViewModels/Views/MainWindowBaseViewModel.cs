using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.BaseViewModels.Views
{
    public class MainWindowBaseViewModel : ANotifyPropertyChanged
    {
        static MainWindowBaseViewModel This { get; set; }
        public MainWindowBaseViewModel()
        {
            This = this;
        }

        bool _IsLoginPage = true;

        public bool IsLoginPage
        {
            get
            {
                return _IsLoginPage;
            }
            set
            {
                _IsLoginPage = value;
                OnPropertyChanged("IsLoginPage");
            }
        }

        bool _IsChatPage = false;
        public bool IsChatPage
        {
            get
            {
                return _IsChatPage;
            }
            set
            {
                _IsChatPage = value;
                OnPropertyChanged("IsChatPage");
            }
        }


        public static void ShowPage(bool isLoginPage = false, bool isChatPage = false)
        {
            This.IsLoginPage = isLoginPage;
            This.IsChatPage = isChatPage;
        }
    }
}
