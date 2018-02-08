using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NoorpodConversation.ViewModels.Helpers
{
    /// <summary>
    /// مدیریت سایز کنترل ها و بایند کردن
    /// </summary>
    public static class SizeObserver
    {
        /// <summary>
        /// بایند کردن یک کنترل
        /// </summary>
        public static readonly DependencyProperty ObserveControlProperty = DependencyProperty.RegisterAttached(
            "ObserveControl",
            typeof(bool),
            typeof(SizeObserver),
            new FrameworkPropertyMetadata(OnObserveControlChanged));

        private static void OnObserveControlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;
            if (e.NewValue != null)
            {
                dynamic dataContext = frameworkElement.DataContext;
               var lst = ViewsUtility.FindParent<System.Windows.Controls.ListBox>(frameworkElement);
               dataContext.ListBox = lst;
            }
        }

        /// <summary>
        /// دریافت وضعیت بایند یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <returns></returns>
        public static bool GetObserveControl(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(ObserveControlProperty);
        }
        /// <summary>
        /// تغییر وضعیت بایند یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <param name="observe"></param>
        public static void SetObserveControl(FrameworkElement frameworkElement, bool observe)
        {
            frameworkElement.SetValue(ObserveControlProperty, observe);
        }

        /// <summary>
        /// بایند کردن یک کنترل
        /// </summary>
        public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(SizeObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));
        /// <summary>
        /// بایند کردن عرض
        /// </summary>
        public static readonly DependencyProperty ObservedWidthProperty = DependencyProperty.RegisterAttached(
            "ObservedWidth",
            typeof(double),
            typeof(SizeObserver));
        /// <summary>
        /// بایند کردن ارتفاع
        /// </summary>
        public static readonly DependencyProperty ObservedHeightProperty = DependencyProperty.RegisterAttached(
            "ObservedHeight",
            typeof(double),
            typeof(SizeObserver));
        /// <summary>
        /// دریافت وضعیت بایند یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <returns></returns>
        public static bool GetObserve(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(ObserveProperty);
        }
        /// <summary>
        /// تغییر وضعیت بایند یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <param name="observe"></param>
        public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        {
            frameworkElement.SetValue(ObserveProperty, observe);
        }

        /// <summary>
        /// دریافت اندازه ی عرض یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <returns></returns>
        public static double GetObservedWidth(FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.GetValue(ObservedWidthProperty);
        }
        /// <summary>
        /// تغییر اندازه ی عرض یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <param name="observedWidth"></param>
        public static void SetObservedWidth(FrameworkElement frameworkElement, double observedWidth)
        {
            frameworkElement.SetValue(ObservedWidthProperty, observedWidth);
        }
        /// <summary>
        /// دریافت اندازه ی ارتفاع یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <returns></returns>
        public static double GetObservedHeight(FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.GetValue(ObservedHeightProperty);
        }
        /// <summary>
        /// ارسال اندازه ی ارتفاع یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <param name="observedHeight"></param>
        public static void SetObservedHeight(FrameworkElement frameworkElement, double observedHeight)
        {
            frameworkElement.SetValue(ObservedHeightProperty, observedHeight);
        }
        /// <summary>
        /// زمانی که بایند کردن کنترل انجام شد یا تغییر کرد
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
                UpdateObservedSizesForFrameworkElement(frameworkElement);
            }
            else
            {
                frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
            }
        }
        /// <summary>
        /// زمانی که اندازه ی کنترل تغییر کرد
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
        }
        /// <summary>
        /// انجام تغییرات روی بایندینگ
        /// </summary>
        /// <param name="frameworkElement"></param>
        private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
        {
            // WPF 4.0 onwards
            frameworkElement.SetCurrentValue(ObservedWidthProperty, frameworkElement.ActualWidth);
            frameworkElement.SetCurrentValue(ObservedHeightProperty, frameworkElement.ActualHeight);

            // WPF 3.5 and prior
            ////SetObservedWidth(frameworkElement, frameworkElement.ActualWidth);
            ////SetObservedHeight(frameworkElement, frameworkElement.ActualHeight);
        }
    }


    /// <summary>
    /// مدیریت سایز کنترل ها و بایند کردن
    /// </summary>
    public static class MouseObserver
    {
        /// <summary>
        /// بایند کردن یک کنترل
        /// </summary>
        public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(MouseObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));
        /// <summary>
        /// بایند کردن عرض
        /// </summary>
        public static readonly DependencyProperty ObservedIsMouseOverProperty = DependencyProperty.RegisterAttached(
            "ObservedIsMouseOver",
            typeof(bool),
            typeof(MouseObserver));
        /// <summary>
        /// دریافت وضعیت بایند یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <returns></returns>
        public static bool GetObserve(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(ObserveProperty);
        }
        /// <summary>
        /// تغییر وضعیت بایند یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <param name="observe"></param>
        public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        {
            frameworkElement.SetValue(ObserveProperty, observe);
        }

        /// <summary>
        /// دریافت اندازه ی عرض یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <returns></returns>
        public static bool GetObservedIsMouseOver(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(ObservedIsMouseOverProperty);
        }
        /// <summary>
        /// تغییر اندازه ی عرض یک کنترل
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <param name="observedWidth"></param>
        public static void SetObservedIsMouseOver(FrameworkElement frameworkElement, bool isMouseOver)
        {
            frameworkElement.SetValue(ObservedIsMouseOverProperty, isMouseOver);
        }
      
        /// <summary>
        /// زمانی که بایند کردن کنترل انجام شد یا تغییر کرد
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                frameworkElement.MouseMove += frameworkElement_MouseMove;
                frameworkElement.MouseLeave += frameworkElement_MouseMove;
                UpdateObservedSizesForFrameworkElement(frameworkElement);
            }
            else
            {
                frameworkElement.MouseMove -= frameworkElement_MouseMove;
                frameworkElement.MouseLeave -= frameworkElement_MouseMove;
            }
        }

        static void frameworkElement_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
        }

        /// <summary>
        /// انجام تغییرات روی بایندینگ
        /// </summary>
        /// <param name="frameworkElement"></param>
        private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
        {
            frameworkElement.SetCurrentValue(ObservedIsMouseOverProperty, frameworkElement.IsMouseOver);
        }
    }
}
