using NoorpodConversation.BaseViewModels.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.ViewModels.Views
{
    public class LoginPageViewModel : LoginPageBaseViewModel
    {
        public LoginPageViewModel()
        {
            LoginCommand = new RelayCommand(Register, ()=>
            {
                return !HasError(0);
            });
        }

        public RelayCommand LoginCommand { get; set; }
    }
}
