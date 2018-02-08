using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace NoorpodConversation.ViewModels.Helpers
{
    /// <summary>
    /// کلاس راهنمای انیمیشن و تغییر یک کنترل با انیمیشن
    /// </summary>
    public class AnimationsHelper
    {
        /// <summary>
        /// تغییر حالت یک کنترل با انیمیشن
        /// </summary>
        /// <param name="nowChange">زمانی که انیمشن اول تمام شد این اکشن اجرا میشود</param>
        /// <param name="element">کنترلی که انیمیشن روی ان اجرا میشود</param>
        /// <param name="startAnimation">انیمیشن شروع</param>
        /// <param name="stopAnimation">انیمیشن پایان</param>
        public static void ChangeWithAnimation(Action nowChange, FrameworkElement element, Storyboard startAnimation, Storyboard stopAnimation)
        {
            AnimationsHelper animation = new AnimationsHelper();
            animation.Start(nowChange, element, startAnimation, stopAnimation);
        }

        Storyboard _startAnimation;

        /// <summary>
        /// انیمیشن ایست
        /// </summary>
        Storyboard _stopAnimation;
        /// <summary>
        /// اکشن تغییر حالا
        /// </summary>
        Action _nowChange = null;
        /// <summary>
        /// کنترل تغییر انیمیشن
        /// </summary>
        FrameworkElement _element;
        /// <summary>
        /// شروع انیمیشن
        /// </summary>
        /// <param name="nowChange">اکشن تغییر</param>
        /// <param name="element">کنترلی که میخواهید به ان انیمیشن دهید</param>
        /// <param name="startAnimation">انیمیشن شروع</param>
        /// <param name="stopAnimation">انیمیشن پایان</param>
        public void Start(Action nowChange, FrameworkElement element, Storyboard startAnimation, Storyboard stopAnimation)
        {
            _startAnimation = startAnimation.Clone();
            _stopAnimation = stopAnimation.Clone();
            _element = element;
            _nowChange = nowChange;
            // _startAnimation.Completed -= sb_Completed;
            //_startAnimation.Completed += sb_Completed;
            //foreach (var item in startAnimation.Children)
            //{
            //    EventHandler complete = (ss, ee) =>
            //    {

            //    };

            //    item.Completed -= complete;
            //    item.Completed += complete;
            //}
            // _startAnimation.Changed -= _startAnimation_Changed;


            //_startAnimation.Begin(element, HandoffBehavior.Compose, true);

            _element.BeginStoryboard(_startAnimation, HandoffBehavior.Compose, true);
            CompletedAnimationChecker.CheckComplete(_element, _startAnimation, () =>
            {
                _nowChange();
                _element.BeginStoryboard(_stopAnimation);
            });
        }



        //static List<string> coms = new List<string>();
        ///// <summary>
        ///// اتمام انیمیشن
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void sb_Completed(object sender, EventArgs e)
        //{
        //    completes = true;
        //    var p = _startAnimation.GetCurrentProgress(_element);

        //}
    }

    public static class CompletedAnimationChecker
    {
        public static void CheckComplete(FrameworkElement element, Storyboard story, Action complete)
        {
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(30);
                        var p = story.GetCurrentProgress(element);
                        if (p == 1.0)
                        {
                            System.ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                            {
                                complete();
                            });
                            break;
                        }
                    }
                    catch
                    {

                    }
                }
            });
            task.Start();
        }
    }
}
