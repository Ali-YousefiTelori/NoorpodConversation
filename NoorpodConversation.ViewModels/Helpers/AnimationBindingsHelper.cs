using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace NoorpodConversation.ViewModels.Helpers
{
    /// <summary>
    /// کلاس کمک کننده ی بایندینگ انیمیشن
    /// </summary>
    public class AnimationBindingsHelper
    {
        /// <summary>
        /// بایند کردن دستی انیمیشن
        /// </summary>
        public static readonly DependencyProperty ManualAnimationBindingProperty = DependencyProperty.RegisterAttached(
            "ManualAnimationBinding",
            typeof(bool),
            typeof(AnimationBindingsHelper),
            new FrameworkPropertyMetadata(ManualAnimationBindingChanged));

        /// <summary>
        /// دریافت وضعیت بایند
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetManualAnimationBinding(object element)
        {
            return false;
        }
        /// <summary>
        /// ارسال وضعیت بایند
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="value"></param>
        public static void SetManualAnimationBinding(object dependencyObject, bool value)
        {

        }

        /// <summary>
        /// تغییر وضعیت بایند شده
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="e"></param>
        private static void ManualAnimationBindingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is Expander)
            {
                var element = dependencyObject as Expander;
                element.Loaded += element_Loaded;

            }
        }
        static List<MyLabel> list = new List<MyLabel>();

        /// <summary>
        /// زمانی که کنترل لود و بارگزاری شد
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void element_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var element = sender as Expander;
                //var template = element.Template as ControlTemplate;
                if (System.ApplicationHelper.Current == null)
                    return;
                MyLabel label = new MyLabel();
                Binding binding = new Binding();
                binding.Path = new PropertyPath("IsExpanded");
                binding.Source = element;
                binding.Mode = BindingMode.TwoWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                label.SetBinding(MyLabel.ContentProperty, binding);
                list.Add(label);
                //BindingOperations.SetBinding(element, Expander.IsExpandedProperty, binding);

                //foreach (Trigger trigger in template.Triggers)
                //{
                //    foreach (var beginStoryboard in trigger.EnterActions)
                //    {
                //        if (beginStoryboard is BeginStoryboard)
                //        {
                //            foreach (var child in ((BeginStoryboard)beginStoryboard).Storyboard.Children)
                //            {
                //                if (child is DoubleAnimationUsingKeyFrames)
                //                {
                //                    foreach (var keyFrame in ((DoubleAnimationUsingKeyFrames)child).KeyFrames)
                //                    {
                //                        if (keyFrame is EasingDoubleKeyFrame)
                //                        {
                //                            var frame = keyFrame as EasingDoubleKeyFrame;
                //                            frame.Value = 200.0;
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }

                //}
                //var manualBeginStoryboard = NoorpodConversation.BaseViewModels.Helpers.ApplicationHelper.Current.GetAppResource<BeginStoryboard>("manualBeginStoryboard");
                //foreach (var child in manualBeginStoryboard.Storyboard.Children)
                //{
                //    if (child is DoubleAnimationUsingKeyFrames)
                //    {
                //        foreach (var keyFrame in ((DoubleAnimationUsingKeyFrames)child).KeyFrames)
                //        {
                //            if (keyFrame is EasingDoubleKeyFrame)
                //            {
                //                var frame = keyFrame as EasingDoubleKeyFrame;

                //            }
                //        }
                //    }
                //}
                //var manualBeginStoryboard = NoorpodConversation.BaseViewModels.Helpers.ApplicationHelper.Current.GetAppResource<Storyboard>("manualBeginStoryboard");
                //foreach (var child in manualBeginStoryboard.Children)
                //{
                //    if (child is DoubleAnimationUsingKeyFrames)
                //    {
                //        foreach (var keyFrame in ((DoubleAnimationUsingKeyFrames)child).KeyFrames)
                //        {
                //            if (keyFrame is EasingDoubleKeyFrame)
                //            {
                //                var frame = keyFrame as EasingDoubleKeyFrame;
                //                frame.Value = 300.0;
                //            }
                //        }
                //    }
                //}

                //NoorpodConversation.BaseViewModels.Helpers.ApplicationHelper.Current.SetAppResource("manualBeginStoryboard", manualBeginStoryboard);
                //Trigger trigger = new Trigger();
                //trigger.Property = Expander.IsExpandedProperty;
                //trigger.Value = true;
                //trigger.EnterActions.Add(manualBeginStoryboard);
                //template.Triggers.Add(trigger);
            }
            catch (Exception ex)
            {

            }
        }
    }

    /// <summary>
    /// یک برچسب خصوصی سازی شده
    /// </summary>
    public class MyLabel : ContentControl
    {

        static MyLabel()
        {
            ContentProperty.OverrideMetadata(typeof(MyLabel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnContentChanged), null, true, UpdateSourceTrigger.PropertyChanged));
        }


        /// <summary>
        /// تغییر داده ی متن برچسب
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnContentChanged(DependencyObject d,

            DependencyPropertyChangedEventArgs e)
        {

            MyLabel mcc = d as MyLabel;
            var name = e.NewValue;
            //var bindingExpression = mcc.GetBindingExpression(ContentProperty);

            //if (bindingExpression == null)
            //{
            //    mcc.SetValue(ContentProperty, e.NewValue);
            //}
            //else
            //{
            //    var target = bindingExpression.DataItem;

            //    var propertyInfo = target.GetType().GetProperty(bindingExpression.ParentBinding.Path.Path);

            //    propertyInfo.SetValue(target, e.NewValue, null);
            //}
        }
    }
}
