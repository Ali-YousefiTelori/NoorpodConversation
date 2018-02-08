using NoorpodInforming.Views;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace NoorpodInforming
{
    public static class InformingManager
    {
        static InformingManager()
        {
            ShowFromQueue();
        }

        static List<InformingMessage> Messages = new List<InformingMessage>();

        static Thickness _WindowMargin = new Thickness(0, 0, 10, 10);
        static int _maxCount;
        static double _left = 0.0;
        static ManualResetEvent resetEvent = new ManualResetEvent(false);
        static double windowHeight = 0.0;
        public static void Show(this InformingMessage informingMessage)
        {
            if (informingMessage.Dispatcher != null && informingMessage.Dispatcher != System.Windows.Threading.Dispatcher.CurrentDispatcher)
            {
                ApplicationHelper.Current.EnterDispatcherThreadFromCustomDispatcher(() =>
                {
                    Show(informingMessage);
                }, informingMessage.Dispatcher);
                return;
            }
            NotifyMessageWindow window = new NotifyMessageWindow() { DataContext = informingMessage };
            windowHeight = window.Height;
            informingMessage.IsShowedChangedAction = () =>
            {
                resetEvent.Set();
            };
            window.Topmost = true;
            informingMessage.MainWindow = window;
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            _left = desktopWorkingArea.Right - window.Width - _WindowMargin.Right;
            window.Left = desktopWorkingArea.Right - _WindowMargin.Right;
            _maxCount = 1;// (int)(desktopWorkingArea.Bottom / (window.Height + _WindowMargin.Bottom));
            bool mustClose = false;
            bool animating = false;
            window.Closing += (s, e) =>
            {
                if (animating && !mustClose)
                {
                    e.Cancel = true;
                    return;
                }
                else if (mustClose)
                {
                    informingMessage.IsDisposed = true;
                    informingMessage.IsShowed = false;
                    e.Cancel = false;
                    return;
                }
                else
                {
                    e.Cancel = true;
                    animating = true;
                    DoubleAnimation animationClose = new DoubleAnimation(desktopWorkingArea.Right, new Duration(new TimeSpan(0, 0, 0, 0, 300)));
                    animationClose.AccelerationRatio = 1;
                    animationClose.Completed += (ss, ee) =>
                    {
                        mustClose = true;
                        window.Close();
                    };
                    window.BeginAnimation(Window.LeftProperty, animationClose);
                }
            };
            Messages.Add(informingMessage);
            resetEvent.Set();
        }

        static double GetFreeTop()
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            double workAreaHeight = desktopWorkingArea.Bottom - (windowHeight + _WindowMargin.Bottom);
            List<double> allTops = new List<double>();
            for (int i = _maxCount; i > 0; i--)
            {
                allTops.Add(workAreaHeight);
                workAreaHeight -= windowHeight + _WindowMargin.Bottom;
            }

            var showedItem = (from x in Messages where x.IsShowed select x).ToList();
            foreach (var item in showedItem)
            {
                allTops.Remove(((Window)item.MainWindow).Top);
            }
            return allTops.Max();
        }

        static void ShowNow(InformingMessage informingMessage)
        {
            if (informingMessage.Dispatcher != null && informingMessage.Dispatcher != System.Windows.Threading.Dispatcher.CurrentDispatcher)
            {
                ApplicationHelper.Current.EnterDispatcherThreadFromCustomDispatcher(() =>
                {
                    ShowNow(informingMessage);
                }, informingMessage.Dispatcher);
                return;
            }
            ((Window)informingMessage.MainWindow).Top = GetFreeTop();
            ((Window)informingMessage.MainWindow).Show();
            informingMessage.IsShowed = true;
            DoubleAnimation animation = new DoubleAnimation(_left, new Duration(new TimeSpan(0, 0, 0, 0, 300)));
            animation.DecelerationRatio = 1;
            ((Window)informingMessage.MainWindow).BeginAnimation(Window.LeftProperty, animation);
            Task tsk = new Task(() =>
            {
                System.Threading.Thread.Sleep(3000);
                ((Window)informingMessage.MainWindow).Dispatcher.Invoke(new Action(() =>
                {
                    ((Window)informingMessage.MainWindow).Close();
                }));
            });
            tsk.Start();
        }

        static void ShowFromQueue()
        {
            Task tsk = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        var showedItem = (from x in Messages where x.IsShowed select x).ToList();
                        var notShowedItem = (from x in Messages where !x.IsShowed && !x.IsDisposed select x).ToList();
                        if (notShowedItem.Count > 0 && showedItem.Count < _maxCount)
                        {
                            var first = notShowedItem.FirstOrDefault();
                            if (first != null)
                            {
                                ((Window)first.MainWindow).Dispatcher.Invoke(new Action(() =>
                                {
                                    ShowNow(first);
                                }));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "InformingManager ShowFromQueue");
                    }
                    resetEvent.WaitOne();
                    resetEvent.Reset();
                }
            });
            tsk.Start();
        }

        public static void Sucess(this InformingMessage informingMessage, string title)
        {
            informingMessage.Status = InformingState.Success;
            informingMessage.Title = title;
            Task tsk = new Task(() =>
            {
                System.Threading.Thread.Sleep(2000);
                ((Window)informingMessage.MainWindow).Dispatcher.Invoke(new Action(() =>
                {
                    ((Window)informingMessage.MainWindow).Close();
                }));
            });
            tsk.Start();
        }

        public static void Fail(this InformingMessage informingMessage, string title)
        {
            informingMessage.Status = InformingState.Fail;
            informingMessage.Title = title;
            Task tsk = new Task(() =>
            {
                System.Threading.Thread.Sleep(5000);
                ((Window)informingMessage.MainWindow).Dispatcher.Invoke(new Action(() =>
                {
                    ((Window)informingMessage.MainWindow).Close();
                }));
            });
            tsk.Start();
        }

        public static void InitializeBase()
        {
            InformingMessageExtension.FailAction = (info,title) =>
            {
                Fail(info, title);
            };
            InformingMessageExtension.SuccessAction = (info, title) =>
            {
                Sucess(info, title);
            };
            InformingMessageExtension.ShowMessageAction = (info) =>
            {
                Show(info);
            };
        }
    }
}
