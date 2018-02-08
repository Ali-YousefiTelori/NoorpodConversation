using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoorpodInforming.Views
{
    /// <summary>
    /// Interaction logic for NotifyWindow.xaml
    /// </summary>
    public partial class NotifyWindow : Window
    {
        public NotifyWindow()
        {
            InitializeComponent();
        }

        //protected override void OnActivated(EventArgs e)
        //{
        //    base.OnActivated(e);

        //    //Set the window style to noactivate.
        //    WindowInteropHelper helper = new WindowInteropHelper(this);
        //    SetWindowLong(helper.Handle, GWL_EXSTYLE,
        //        GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        //}

        //private const int GWL_EXSTYLE = -20;
        //private const int WS_EX_NOACTIVATE = 0x08000000;

        //[DllImport("user32.dll")]
        //public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //[DllImport("user32.dll")]
        //public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    }
}
