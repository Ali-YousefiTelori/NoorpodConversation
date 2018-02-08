using NoorpodConversation.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace NoorpodConversation.UI.Views.Controls
{
    public class PopupAutoLocation : Popup
    {
        public PopupAutoLocation()
        {
            if (ViewsUtility.MainWindow == null)
                return;
            ViewsUtility.MainWindow.LocationChanged += This_LocationChanged;
            ViewsUtility.MainWindow.SizeChanged += This_LocationChanged;
            this.Loaded += PopupAutoLocation_Loaded;
            this.LayoutUpdated += This_LocationChanged;
        }

        void PopupAutoLocation_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.PlacementTarget == null)
                return;
            ((FrameworkElement)this.PlacementTarget).SizeChanged += This_LocationChanged;
        }

        void This_LocationChanged(object sender, EventArgs e)
        {
            this.HorizontalOffset += 0.1;
            this.HorizontalOffset -= 0.1;
        }
    }
}
