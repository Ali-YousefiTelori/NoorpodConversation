using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace NoorpodConversation.ViewModels.Helpers
{
    public class CustomDataTrigger : DataTrigger
    {
        //public static readonly DependencyProperty ElementProperty = DependencyProperty.RegisterAttached("Element", typeof(FrameworkElement), typeof(CustomDataTrigger), new FrameworkPropertyMetadata(OnChanged));

        //private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}

        string _CustomResourceName = "";
        public string CustomResourceName
        {
            get
            {
                return _CustomResourceName;
            }
            set
            {
                _CustomResourceName = value;
            }
        }
    }

    public static class AnimationActionRunner
    {
        /// </summary>
        //public static readonly DependencyProperty ActionRunnerProperty = DependencyProperty.RegisterAttached(
        //    "ActionRunner",
        //    typeof(object),
        //    typeof(AnimationActionRunner),
        //    new FrameworkPropertyMetadata(OnActionRunnerChanged));

        //private static void OnActionRunnerChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        //{
        //    var frameworkElement = (FrameworkElement)dependencyObject;
        //    if (e.NewValue != null)
        //    {
        //        frameworkElement.Loaded += frameworkElement_Loaded;
        //        //ANotifyPropertyChanged apc = (frameworkElement.DataContext as ANotifyPropertyChanged);
        //        //apc.StoryBoardComeplete = e.NewValue as Action;
        //        //dynamic dataContext = frameworkElement.DataContext;
        //        //var lst = ViewsUtility.FindParent<System.Windows.Controls.ListBox>(frameworkElement);
        //        //dataContext.ListBox = lst;
        //    }
        //}

        //static Dictionary<Storyboard, FrameworkElement> Items = new Dictionary<Storyboard, FrameworkElement>();
        //static void frameworkElement_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var frameworkElement = (FrameworkElement)sender;
        //    if (NoorpodConversation.BaseViewModels.Helpers.ApplicationHelper.Current == null || frameworkElement == null || frameworkElement.Style == null)
        //        return;
        //    var triggert = frameworkElement.Style.Triggers[0] as DataTrigger;
        //    triggert.EnterActions.Clear();
        //    BeginStoryboard begin = new BeginStoryboard();
        //    var story = NoorpodConversation.BaseViewModels.Helpers.ApplicationHelper.Current.GetAppResource<Storyboard>("NormalAnimationStart");
        //    if (story == null)
        //        return;
        //    begin.Storyboard = story;

        //    begin.Storyboard.Completed += (s, ee) =>
        //        {
        //            Task task = new Task(() =>
        //            {
        //                System.Threading.Thread.Sleep(30);
        //                NoorpodConversation.BaseViewModels.Helpers.ApplicationHelper.Current.EnterDispatcherThreadActionBeginRenderMode(() =>
        //                {
        //                    (Items[story].DataContext as ANotifyPropertyChanged).StoryBoardComepleteAction();
        //                });
        //            });
        //            task.Start();
        //        };
        //    triggert.EnterActions.Add(begin);
        //    Items.Add(begin.Storyboard, frameworkElement);
        //}

        ///// <summary>
        ///// دریافت وضعیت بایند یک کنترل
        ///// </summary>
        ///// <param name="frameworkElement"></param>
        ///// <returns></returns>
        //public static object GetActionRunner(FrameworkElement frameworkElement)
        //{
        //    return true;
        //}
        ///// <summary>
        ///// تغییر وضعیت بایند یک کنترل
        ///// </summary>
        ///// <param name="frameworkElement"></param>
        ///// <param name="observe"></param>
        //public static void SetActionRunner(FrameworkElement frameworkElement, object observe)
        //{
        //    // frameworkElement.SetValue(ActionRunnerProperty, observe);
        //}


        public static readonly DependencyProperty DataTriggerProperty = DependencyProperty.RegisterAttached(
            "DataTrigger",
            typeof(CustomDataTrigger),
            typeof(AnimationActionRunner),
            new FrameworkPropertyMetadata(OnDataTriggerChanged));

        //static List<string> items = new List<string>();
        private static void OnDataTriggerChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var frameworkElement = dependencyObject as FrameworkElement;
                if (e.NewValue != null)
                {
                    var dataTrigger = e.NewValue as CustomDataTrigger;
                    var binding = dataTrigger.Binding as System.Windows.Data.Binding;
                    var pc = binding.Source as ANotifyPropertyChanged;
                    var elemet = CustomResources.GetResource(dataTrigger.CustomResourceName);
                    if (pc != null)
                    {
                        pc.PropertyChanged += (o, pName) =>
                        {
                            if (pName.PropertyName == binding.Path.Path)
                            {
                                if (elemet == null)
                                {
                                    Action<string> addedAction = null;
                                    addedAction = (rName) =>
                                    {
                                        elemet = CustomResources.GetResource(dataTrigger.CustomResourceName);
                                        if (elemet == null)
                                            return;
                                        //if (pName.PropertyName == rName)
                                        //{
                                        pc.OnPropertyChanged(pName.PropertyName);
                                        CustomResources.AddedAction -= addedAction;
                                        //}
                                    };
                                    CustomResources.AddedAction -= addedAction;
                                    CustomResources.AddedAction += addedAction;
                                    return;
                                }
                                //items.Add("PropertyChanged 1");
                                if (dataTrigger.Value.ToString().ToLower() == ViewsUtility.GetPropertyValue<object>(pc, pName.PropertyName).ToString().ToLower())
                                {
                                    //items.Add("PropertyChanged 2");
                                    EventHandler complete = (s, ee) =>
                                    {
                                        //items.Add("PropertyChanged 5");
                                        //Task task = new Task(() =>
                                        //{
                                        //    System.Threading.Thread.Sleep(30);
                                        //    NoorpodConversation.BaseViewModels.Helpers.ApplicationHelper.Current.EnterDispatcherThreadActionBeginRenderMode(() =>
                                        //    {
                                        
                                        var pchanged = elemet.DataContext as ANotifyPropertyChanged;
                                        if (pchanged == null || pchanged.StoryBoardComepleteAction == null)
                                            return; 
                                        pchanged.StoryBoardComepleteAction();
                                        //    });
                                        //});
                                        //task.Start();
                                    };

                                    foreach (var story in dataTrigger.EnterActions)
                                    {
                                        //items.Add("PropertyChanged 3");
                                        BeginStoryboard begin = story as BeginStoryboard;
                                        //begin.Storyboard.Completed -= complete;
                                        //begin.Storyboard.Completed += complete;
                                        begin.Storyboard.Begin(frameworkElement, true);
                                        CompletedAnimationChecker.CheckComplete(frameworkElement, begin.Storyboard, () =>
                                        {
                                            complete(null, null);
                                        });
                                        //items.Add("PropertyChanged 4");
                                    }
                                }
                                else
                                {
                                    //items.Add("PropertyChanged S");
                                    foreach (var story in dataTrigger.ExitActions)
                                    {
                                        BeginStoryboard begin = story as BeginStoryboard;
                                        begin.Storyboard.Begin(frameworkElement);
                                    }
                                }
                            }
                        };
                        pc.OnPropertyChanged(binding.Path.Path);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static object GetDataTrigger(FrameworkElement frameworkElement)
        {
            return true;
        }

        public static void SetDataTrigger(FrameworkElement frameworkElement, CustomDataTrigger dataTrigger)
        {
            // frameworkElement.SetValue(ActionRunnerProperty, observe);
        }
    }

    public static class CustomResources
    {
        public static event Action<string> AddedAction;
        static Dictionary<string, FrameworkElement> items = new Dictionary<string, FrameworkElement>();
        public static readonly DependencyProperty AddResourceProperty = DependencyProperty.RegisterAttached(
            "AddResource",
            typeof(string),
            typeof(CustomResources),
            new FrameworkPropertyMetadata(OnAddResourceChanged));

        private static void OnAddResourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetAddResource((FrameworkElement)d, (string)e.NewValue);
        }

        public static string GetAddResource(FrameworkElement frameworkElement)
        {
            return null;
        }

        public static void SetAddResource(FrameworkElement frameworkElement, string resourceName)
        {
            items.Add(resourceName, frameworkElement);
            if (AddedAction != null)
                AddedAction(resourceName);
        }

        public static FrameworkElement GetResource(string key)
        {
            return items.ContainsKey(key) ? items[key] : null;
        }
    }
}
