using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace NoorpodConversation.ViewModels.Helpers
{
    /// <summary>
    /// کمک کننده ی رابط کاربری
    /// </summary>
    public static class ViewsUtility
    {
        /// <summary>
        /// پنجره ی اصلی نرم افزار
        /// </summary>
        public static Window MainWindow { get; set; }
        /// <summary>
        /// پیدا کردن یک کنترل پدر بر اساس نوع آن
        /// </summary>
        /// <typeparam name="T">نوع کلاس مورد نظر برای پیدا کردن</typeparam>
        /// <param name="child">کنترلی که میخواهید پدر های آن بررسی شود</param>
        /// <returns>مقدار بازگشتی کنترل پدر یافت شده می باشد</returns>
        public static T FindParent<T>(FrameworkElement child) where T : DependencyObject
        {
            T parent = null;
            var currentParent = VisualTreeHelper.GetParent(child);

            while (currentParent != null)
            {
                if (currentParent is T)
                {
                    parent = (T)currentParent;
                    break;
                }
                var newP = VisualTreeHelper.GetParent(currentParent);
                if (newP == null)
                {
                    newP = (currentParent as FrameworkElement).Parent;
                    if (newP == null)
                    {
                        newP = (currentParent as FrameworkElement).TemplatedParent;
                    }
                }
                currentParent = newP;
            }

            return parent;
        }

        /// <summary>
        /// پیدا کردن محل یک کنترل روی صفحه
        /// </summary>
        /// <param name="element">کنترل مورد نظر</param>
        /// <param name="pointToElement">کنترل پدر که کنترل اصلی در آن وجود دارد</param>
        /// <param name="relativeToScreen">بر اساس اسکرین کنرل معمولی محل را پیدا می کند</param>
        /// <returns></returns>
        public static Rect GetAbsoltutePlacement(FrameworkElement element, FrameworkElement pointToElement, bool relativeToScreen = false)
        {
            var absolutePos = element.TranslatePoint(new System.Windows.Point(0, 0), pointToElement);
            double width = double.IsNaN(element.Width) ? element.ActualWidth : element.Width;
            double height = double.IsNaN(element.Height) ? element.ActualHeight : element.Height;
            if (relativeToScreen)
            {
                return new Rect(absolutePos.X, absolutePos.Y, width, height);
            }
            var posMW = Application.Current.MainWindow.PointToScreen(new System.Windows.Point(0, 0));
            absolutePos = new System.Windows.Point(absolutePos.X - posMW.X, absolutePos.Y - posMW.Y);
            return new Rect(absolutePos.X, absolutePos.Y, width, height);
        }

        public static T GetPropertyValue<T>(object src, string propName)
        {
            var val = (T)src.GetType().GetProperty(propName).GetValue(src, null);
            return val;
        }
    }
}
