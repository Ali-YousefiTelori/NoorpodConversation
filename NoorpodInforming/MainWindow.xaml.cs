using System;
using System.Collections.Generic;
using System.Windows;

namespace NoorpodInforming
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<InformingMessage> msgs = new List<InformingMessage>();
        public MainWindow()
        {
            InitializeComponent();

            //for (int i = 0; i < 20; i++)
            //{

            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //InformingMessage msg = new InformingMessage("درحال بارگذاری...");
            //msg.Show();
            //msgs.Add(msg);

            //for (int i = 0; i < 10; i++)
            //{
            //    msgs[i].Sucess();
            //}
        }

        bool sf = false;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in msgs)
            {
                sf = !sf;
                if (sf)
                    item.Sucess("با موفقیت انجام شد.");
                else
                    item.Fail("عدم دسترسی به سرور رخ داده است");
            }
            msgs.Clear();
        }
    }
}
