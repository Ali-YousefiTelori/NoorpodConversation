using NoorpodConversation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoorpodConversation.UI.Views
{
    /// <summary>
    /// Interaction logic for RoomManager.xaml
    /// </summary>
    public partial class RoomManager : Window
    {
        public RoomManager()
        {
            InitializeComponent();
            AsyncRunner.Run(() =>
            {
                var result = NoorpodServiceHelper.GetLastRoomMessage();
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    txtRoomMessage.Text = result.Data;
                });
            });
            AsyncRunner.Run(() =>
            {
                var result = NoorpodServiceHelper.GetRoomSubject();
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    txtSubject.Text = result.Data;
                });
            });
            AsyncRunner.Run(() =>
            {
                var last = NoorpodServiceHelper.GetLastSpeeckQaulity();
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    txtbufferMills.Text = last.Data.Item1.ToString();
                    txtSampleRate.Text = last.Data.Item2.ToString();
                });
            });
        }

        private void btnAddNewRoomMessage_Click(object sender, RoutedEventArgs e)
        {
            btnAddNewRoomMessage.IsEnabled = false;
            string msg = txtRoomMessage.Text;
            AsyncRunner.Run(() =>
            {
                var result = NoorpodServiceHelper.AddRoomMessage(msg);
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    MessageBox.Show(result.IsSuccess ? "پیغام گروه تغییر کرد" : result.Message);
                });
            });
        }

        private void btnCloseRoom_Click(object sender, RoutedEventArgs e)
        {
            btnCloseRoom.IsEnabled = false;
            AsyncRunner.Run(() =>
            {
                var result = NoorpodServiceHelper.CloseRoom();
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    MessageBox.Show(result.IsSuccess ? "اتاق با موفقیت بسته شد" : result.Message);
                });
            });
        }

        private void btnOpenRoom_Click(object sender, RoutedEventArgs e)
        {
            btnOpenRoom.IsEnabled = false;
            AsyncRunner.Run(() =>
            {
                var result = NoorpodServiceHelper.OpenRoom();
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    MessageBox.Show(result.IsSuccess ? "اتاق با موفقیت باز شد" : result.Message);
                });
            });
        }

        private void btnAddSubject_Click(object sender, RoutedEventArgs e)
        {
            btnAddSubject.IsEnabled = false;
            string subject = txtSubject.Text;
            AsyncRunner.Run(() =>
            {
                var result = NoorpodServiceHelper.ChangeRoomSubject(subject);
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    MessageBox.Show(result.IsSuccess ? "موضوع اتاق تغییر کرد" : result.Message);
                });
            });
        }

        private void btnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            btnSaveSetting.IsEnabled = false;
            try
            {
                int sampleRate = int.Parse(txtSampleRate.Text);
                int buffer = int.Parse(txtbufferMills.Text);
                AsyncRunner.Run(() =>
                {
                    var result = NoorpodServiceHelper.ChangeSpeeckQaulity(buffer, sampleRate);
                    ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                    {
                        MessageBox.Show(result.IsSuccess ? "تنظیمات تغییر کرد" : result.Message);
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnSaveSetting.IsEnabled = true;
            }
        }
    }
}
