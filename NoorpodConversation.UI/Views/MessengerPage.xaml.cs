using NoorpodConversation.BaseViewModels.Views;
using NoorpodConversation.ViewModels.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoorpodConversation.UI.Views
{
    /// <summary>
    /// Interaction logic for MessengerPage.xaml
    /// </summary>
    public partial class MessengerPage : UserControl
    {
        public MessengerPage()
        {
            InitializeComponent();
            MessengerPageBaseViewModel.ScrollToDownNowAction = () =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        if (lstMessage.Items.Count > 0)
                        {
                            var border = (Border)VisualTreeHelper.GetChild(lstMessage, 0);
                            var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                                scrollViewer.ScrollToBottom();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }));
            };
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Key == Key.Enter)
                {
                    if (string.IsNullOrEmpty(txtMessage.Text))
                    {
                        e.Handled = true;
                        return;
                    }
                    var vm = this.DataContext as MessengerPageViewModel;
                    var c = txtMessage.SelectionStart;
                    vm.Message = vm.Message.Insert(txtMessage.SelectionStart, Environment.NewLine);
                    txtMessage.SelectionStart = c + Environment.NewLine.Length;
                }
                return;
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                return;
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(txtMessage.Text))
                {
                    e.Handled = true;
                    return;
                }
                var vm = this.DataContext as MessengerPageViewModel;
                if (vm.CanSendMessage())
                {
                    vm.SendMessage();
                    e.Handled = true;
                }
            }
        }

        private void chkNotification_Click(object sender, RoutedEventArgs e)
        {
            InformingMessage.IsInformingEnabled = chkNotification.IsChecked.Value;
        }

        private void roomManagement_Click(object sender, RoutedEventArgs e)
        {
            RoomManager window = new Views.RoomManager();
            window.ShowDialog();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://noorpods.com");
        }
    }
}
