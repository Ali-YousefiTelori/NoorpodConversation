using NoorpodConversation.BaseViewModels.Views;
using NoorpodConversation.Models;
using NoorpodConversation.Services;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NoorpodConversation.ViewModels.Views
{
    public class MessengerPageViewModel : MessengerPageBaseViewModel
    {
        public MessengerPageViewModel()
        {
            DeleteMessageCommand = new System.RelayCommand<MessageInfo>(DeleteMessage);
            AllowMessagePermissionCommand = new System.RelayCommand<UserClient>(AllowMessagePermission);
            AllowMicrophonePermissionCommand = new System.RelayCommand<UserClient>(AllowMicrophonePermission);
            AllowLoginPermissionCommand = new System.RelayCommand<UserClient>(AllowLoginPermission);
            DenyMessagePermissionCommand = new System.RelayCommand<UserClient>(DenyMessagePermission);
            DenyMicrophonePermissionCommand = new System.RelayCommand<UserClient>(DenyMicrophonePermission);
            DenyLoginPermissionCommand = new System.RelayCommand<UserClient>(DenyLoginPermission);
            ChangeUserAdminCommand = new System.RelayCommand<UserClient>(ChangeUserAdmin);
            HandUpCommand = new System.RelayCommand(HandUp, CanHandUp);
            TalkCommand = new System.RelayCommand(Talk, CanTalk);
            SendMessageCommand = new System.RelayCommand(SendMessage, CanSendMessage);
            SetDefaultMicrophoneCommand = new System.RelayCommand<UserClient>(SetDefaultMicrophone);
            LoginPageBaseViewModel.ShowMessageBoxAndCloseAction = (msg) =>
            {
                NoorpodServiceHelper.ApplicationClosed = true;
                NoorpodServiceHelper.Disconnect();
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    MessageBox.Show(msg, "اتاق بسته است");
                    Application.Current.Shutdown();
                });
            };

            NoorpodServiceHelper.OnRoomClosedAction = () =>
            {
                ChangedPermissions();
                if (!NoorpodServiceHelper.CurrentUser.IsAdmin)
                {
                    NoorpodServiceHelper.Disconnect();
                    ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                    {
                        MessageBox.Show("اتاق توسط ادمین بسته شده است", "اتمام گفتگو");
                        Application.Current.Shutdown();
                    });
                }
            };

            NoorpodServiceHelper.OnRoomClosedAction = () =>
            {
                ChangedPermissions();
                if (!NoorpodServiceHelper.CurrentUser.IsAdmin)
                {
                    NoorpodServiceHelper.Disconnect();
                    ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                    {
                        MessageBox.Show("اتاق توسط ادمین بسته شده است", "اتمام گفتگو");
                        Application.Current.Shutdown();
                    });
                }
            };

            NoorpodServiceHelper.OnApplicationCloseAction = (data) =>
            {
                NoorpodServiceHelper.ApplicationClosed = true;
                NoorpodServiceHelper.Disconnect();
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    MessageBox.Show(data, "اتمام گفتگو");
                    Application.Current.Shutdown();
                });
            };

            NoorpodServiceHelper.OnRoomOpenAction = () =>
            {
                ChangedPermissions();
            };
        }
        public RelayCommand<MessageInfo> DeleteMessageCommand { get; set; }
        public RelayCommand<UserClient> AllowMessagePermissionCommand { get; set; }
        public RelayCommand<UserClient> AllowMicrophonePermissionCommand { get; set; }
        public RelayCommand<UserClient> AllowLoginPermissionCommand { get; set; }
        public RelayCommand<UserClient> DenyMessagePermissionCommand { get; set; }
        public RelayCommand<UserClient> DenyMicrophonePermissionCommand { get; set; }
        public RelayCommand<UserClient> DenyLoginPermissionCommand { get; set; }
        public RelayCommand<UserClient> ChangeUserAdminCommand { get; set; }
        public RelayCommand<UserClient> SetDefaultMicrophoneCommand { get; set; }
        public RelayCommand HandUpCommand { get; set; }
        public RelayCommand TalkCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        bool canTalk = true;
        public void Talk()
        {
            if (NoorpodServiceHelper.IsSendAudioNow)
            {
                AsyncRunner.Run(() =>
                {
                    try
                    {
                        NoorpodServiceHelper.SetUserMicrophoneOff(NoorpodServiceHelper.CurrentUser);
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "Talk 2");
                    }
                });
                NoorpodServiceHelper.IsSendAudioNow = false;
                ChangedPermissions();
                canTalk = true;
                return;
            }
            else if (NoorpodServiceHelper.CurrentUser.IsBanMic)
            {
                MessageBox.Show("دسترسی میکروفون شما توسط ادمین قطع شده است");
                canTalk = true;
                ApplicationHelper.Current.RefreshCommandsAction();
                return;
            }
            canTalk = false;

            AsyncRunner.Run(() =>
            {
                try
                {
                    var result = NoorpodServiceHelper.SetDefaultUserMicrophone(NoorpodServiceHelper.CurrentUser);
                    if (result.IsSuccess)
                    {
                        NoorpodServiceHelper.IsSendAudioNow = true;
                        ChangedPermissions();
                        if (IsHandUp)
                            HandUp();
                    }
                    else
                    {
                        ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                        {
                            MessageBox.Show("شما دسترسی اینکار را ندارید لطفا دست خود را بالا برده تا ادمین به شما اجازه دهد");
                        });
                    }
                    canTalk = true;
                    ApplicationHelper.Current.RefreshCommandsAction();
                }
                catch (Exception ex)
                {
                    canTalk = true;
                    AutoLogger.LogError(ex, "Talk");
                    ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                    {
                        MessageBox.Show(ex.Message);
                    });
                }
            });

        }

        private bool CanTalk()
        {
            return canTalk;
        }

        public void DenyLoginPermission(UserClient user)
        {
            if (user.UserName == NoorpodServiceHelper.CurrentUser.UserName)
                return;
            if (MessageBox.Show("آیا از اخراج کاربر اطمینان دارید؟ در صورت اخراج کاربر دیگر نمی تواند وارد نرم افزار شود", "اخراج کاربر", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AsyncRunner.Run(() =>
                {
                    try
                    {
                        NoorpodServiceHelper.DenyPermission(user, PermissionEnum.Login);
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "DenyLoginPermission");
                    }
                });
            }
        }
    }
}
