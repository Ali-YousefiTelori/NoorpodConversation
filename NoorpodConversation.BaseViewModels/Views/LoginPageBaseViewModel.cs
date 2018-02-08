using NoorpodConversation.Models;
using NoorpodConversation.Services;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NoorpodConversation.BaseViewModels.Views
{
    public class LoginPageBaseViewModel : ANotifyPropertyChanged
    {
        public static Action<string> ShowMessageBoxAndCloseAction { get; set; }
        public static Func<UserClient> LoadDataFunction { get; set; }
        public static Action<UserClient> SaveFunction { get; set; }
        public LoginPageBaseViewModel()
        {
            if (ApplicationHelper.Current == null)
                return;
            NoorpodServiceHelper.ReLoginAction = () =>
            {
                MainWindowBaseViewModel.ShowPage(isLoginPage: true);
                NoorpodServiceHelper.Disconnect();
                Login();
            };

            NoorpodServiceHelper.CurrentUser = LoadDataFunction();
            if (NoorpodServiceHelper.CurrentUser != null)
                Login();
        }

        string _BusyMessage;

        public string BusyMessage
        {
            get
            {
                return _BusyMessage;
            }
            set
            {
                _BusyMessage = value;
                OnPropertyChanged("BusyMessage");
            }
        }

        bool _IsBusy = false;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                OnPropertyChanged("UserName");
            }
        }

        string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public void Register()
        {
            if (NoorpodServiceHelper.CurrentUser != null)
            {
                Login();
                return;
            }
            BusyMessage = "در حال اتصال به سرور...";
            IsBusy = true;

            AsyncRunner.Run(() =>
            {
                try
                {
                    NoorpodServiceHelper.Initialize();
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "error Connect: ");
                    BusyMessage = "خطا در اتصال رخ داده است...";
                    Thread.Sleep(3000);
                    Register();
                    return;
                }

                try
                {
                    BusyMessage = "در حال ثبت نام...";

                    var result = NoorpodServiceHelper.Register(UserName, Password);
                    NoorpodServiceHelper.CurrentUser = new UserClient() { UserName = UserName, Password = Password };
                    if (result.IsSuccess)
                    {
                        SaveFunction(NoorpodServiceHelper.CurrentUser);
                        Login();
                    }
                    else
                    {
                        if (result.Message == "Exist")
                        {
                            Login();
                            return;
                        }
                        else
                        {
                            BusyMessage = "خطا در ثبت نام رخ داده است!";
                            Thread.Sleep(2000);
                            IsBusy = false;
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (NoorpodServiceHelper.ApplicationClosed)
                        return;
                    AutoLogger.LogError(ex, "NoorpodServiceHelper.Register");
                    BusyMessage = "خطا در ثبت نام رخ داده است!";
                    Thread.Sleep(2000);
                    IsBusy = false;
                }
            });
        }

        public void Login()
        {
            if (NoorpodServiceHelper.ApplicationClosed)
                return;
            BusyMessage = "در حال ورود...";
            IsBusy = true;
            AsyncRunner.Run(() =>
            {
                BusyMessage = "در حال اتصال به سرور...";
                if (!NoorpodServiceHelper.IsConnected)
                {
                    try
                    {
                        NoorpodServiceHelper.Initialize();
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "error Connect: ");
                        BusyMessage = "خطا در اتصال رخ داده است...";
                        Thread.Sleep(3000);
                        Login();
                        return;
                    }
                }

                BusyMessage = "در حال ورود...";

                try
                {
                    var result = NoorpodServiceHelper.Login(NoorpodServiceHelper.CurrentUser.UserName, NoorpodServiceHelper.CurrentUser.Password);
                    if (result.IsSuccess)
                    {
                        var isRoomOpen = NoorpodServiceHelper.IsRoomOpen();
                        if (isRoomOpen.IsSuccess && !isRoomOpen.Data && !result.Data.Item1.IsAdmin)
                        {
                            NoorpodServiceHelper.ApplicationClosed = true;
                            var msg = NoorpodServiceHelper.GetLastRoomMessage();
                            ShowMessageBoxAndCloseAction?.Invoke(msg.Data);
                            return;
                        }
                        NoorpodServiceHelper.IsRoomOpenned = isRoomOpen.Data;
                        if (result.Data.Item1.IsAdmin)
                            NoorpodServiceHelper.CurrentUser.IsAdmin = true;
                        MessengerPageBaseViewModel.This.Users.Clear();
                        MessengerPageBaseViewModel.This.Users.AddRange(result.Data.Item2);
                        foreach (var item in result.Data.Item2)
                        {
                            item.RefreshIcons();
                        }
                        MainWindowBaseViewModel.ShowPage(isChatPage: true);
                        MessengerPageBaseViewModel.This.ChangedPermissions();

                        var last = NoorpodServiceHelper.GetLastSpeeckQaulity();
                        NoorpodServiceHelper.OnChangedSpeeckQaulityAction?.Invoke(last.Data.Item1, last.Data.Item2);
                        var subject = NoorpodServiceHelper.GetRoomSubject();
                        if (subject.IsSuccess)
                            MessengerPageBaseViewModel.This.RoomSubject = subject.Data;
                    }
                    else
                    {
                        if (result.Message == "room closed")
                        {
                            NoorpodServiceHelper.ApplicationClosed = true;
                            var msg = NoorpodServiceHelper.GetLastRoomMessage();
                            ShowMessageBoxAndCloseAction?.Invoke(msg.Data);
                        }
                        else if (result.Message == "not exist")
                        {
                            BusyMessage = "نام کاربری یا رمز عبور اشتباه است.";
                            Thread.Sleep(3000);
                            IsBusy = false;
                            return;
                        }
                        else
                        {
                            AutoLogger.LogText(result.Message + " stack :" + result.StackTrace);
                            BusyMessage = "خطا در ورود رخ داده است.";
                        }

                        Thread.Sleep(3000);
                        Login();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    if (NoorpodServiceHelper.ApplicationClosed)
                        return;
                    AutoLogger.LogError(ex, "NoorpodServiceHelper.Login");
                    BusyMessage = "خطا در ورود نام رخ داده است!";
                    Thread.Sleep(2000);
                    IsBusy = false;
                }
            });
        }
    }
}
