using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NoorpodConversation.ViewModels.Drawing
{
    public class RenderControlToBitmap
    {
        public static string GetStyleName(DependencyObject obj)
        {
            return (string)obj.GetValue(StyleNameProperty);
        }

        public static void SetStyleName(DependencyObject obj, string value)
        {
            FrameworkElement behavior = obj as FrameworkElement;
            //if (value == "value")
            //{

            //}
            behavior.Loaded += (s, e) =>
            {
                Control parent = behavior.TemplatedParent as Control;
                if (parent == null)
                    return;
                double max, max2;
                if (behavior.ActualHeight < behavior.ActualWidth)
                {
                    max = parent.ActualHeight;
                    max2 = behavior.ActualHeight;
                }
                else
                {
                    max = parent.ActualWidth;
                    max2 = behavior.ActualWidth;
                }

                double scale = 1 * (max / max2);
                behavior.CacheMode = new BitmapCache(scale) { SnapsToDevicePixels = false, EnableClearType = false };
            };
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StyleNameProperty =
            DependencyProperty.RegisterAttached("StyleName", typeof(string), typeof(RenderControlToBitmap),
                new PropertyMetadata(new PropertyChangedCallback(OnIsEnabledPropertyChanged)));

        public static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            SetStyleName(dependencyObject, (string)e.NewValue);
        }
    }
}
