using NoorpodConversation.BaseViewModels.Helpers;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NoorpodConversation.ViewModels.Helpers
{
    /// <summary>
    /// کمک کننده ی انیمیشن
    /// </summary>
    public class ApplicationHelper : IApplicationHelper
    {
        static ApplicationHelper()
        {
            _ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
        public Action RefreshCommandsAction { get; set; }

        static string _ApplicationPath;

        public string ApplicationPath
        {
            get { return _ApplicationPath; }
        }

        /// <summary>
        /// اینیت کردن دیسپچر
        /// </summary>
        /// <param name="dispatcher"></param>
        public static void InitializeDispatcher(object dispatcher)
        {
            if (System.ApplicationHelper.Current == null)
                System.ApplicationHelper.Current = new ApplicationHelper() { DispatcherThread = dispatcher };
        }

        /// <summary>
        /// دیسپچر مورد نظر
        /// </summary>
        public object DispatcherThread { get; set; }

        private string _CurrentLanguage = "_Persian";
        /// <summary>
        /// زبان پیشفرض
        /// </summary>
        public string CurrentLanguage
        {
            get { return _CurrentLanguage; }
            set { _CurrentLanguage = value; }
        }

        /// <summary>
        /// اجرا یک اکشن با دیسپچر مورد نظر
        /// </summary>
        /// <param name="action">اکشن مورد نظر</param>
        public void EnterDispatcherThreadActionForCollections(Action action, object dispatcher)
        {
            if (dispatcher == null)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "EnterDispatcherThreadActionForCollections");
                }
            }
            else
                ((System.Windows.Threading.Dispatcher)dispatcher).BeginInvoke(System.Windows.Threading.DispatcherPriority.DataBind, (Action)delegate
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "EnterDispatcherThreadActionForCollections 2");
                    }
                });
        }


        public void EnterDispatcherThreadFromCustomDispatcher(Action action, object dispatcher)
        {
            if (dispatcher == null)
                return;
            if (((System.Windows.Threading.Dispatcher)dispatcher).Thread == System.Threading.Thread.CurrentThread)
            {
                //Log.AutoLogger.LogText("Lock To Lock Skeep");
                action();
            }
            else
            {
                ((System.Windows.Threading.Dispatcher)dispatcher).BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "EnterDispatcherThreadAction 2");
                    }
                });
            }
        }

        public void SuspendUI(Action action)
        {
            if (System.Windows.Threading.Dispatcher.CurrentDispatcher != (System.Windows.Threading.Dispatcher)DispatcherThread)
            {
                ((System.Windows.Threading.Dispatcher)DispatcherThread).Invoke(new Action(() =>
                {
                    using (var suspend = ((System.Windows.Threading.Dispatcher)DispatcherThread).DisableProcessing())
                    {
                        action();
                    }
                }));
            }
            else
            {
                using (var suspend = ((System.Windows.Threading.Dispatcher)DispatcherThread).DisableProcessing())
                {
                    action();
                }
            }
        }

        /// <summary>
        /// اجرا یک اکشن با دیسپچر مورد نظر
        /// </summary>
        /// <param name="action">اکشن مورد نظر</param>
        public void EnterDispatcherThreadActionBegin(Action action)
        {
            if (DispatcherThread == null)
                return;
            ((System.Windows.Threading.Dispatcher)DispatcherThread).BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "EnterDispatcherThreadActionBegin");
                }
            });
        }
        /// <summary>
        /// من اینو BeginInvoke کردم چون گاهی اوقات هنگ میکرد و لاک میشد.
        /// </summary>
        /// <param name="action">اکشن مورد نظر</param>
        public void EnterDispatcherThreadAction(Action action)
        {
            if (DispatcherThread == null)
                return;
            if (((System.Windows.Threading.Dispatcher)DispatcherThread).Thread == System.Threading.Thread.CurrentThread)
            {
                //Log.AutoLogger.LogText("Lock To Lock Skeep");
                action();
            }
            else
            {
                ((System.Windows.Threading.Dispatcher)DispatcherThread).BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "EnterDispatcherThreadAction");
                    }
                });
            }

            //((System.Windows.Threading.Dispatcher)DispatcherThread).BeginInvoke(action, new TimeSpan(0, 0, 2));
        }



        List<ResourceDictionary> _ResourceDictionaries = new List<ResourceDictionary>();
        /// <summary>
        /// ریسورس های لود شده
        /// </summary>
        public List<ResourceDictionary> ResourceDictionaries
        {
            get { return _ResourceDictionaries; }
            set
            {
                _ResourceDictionaries = value;
            }
        }

        /// <summary>
        /// درج یک ریسورس
        /// </summary>
        /// <param name="address">آدرس ریسورس</param>
        public void AddResourceDictionary(string address)
        {
            if (ContineKey(address))
                return;
            var resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri(address, UriKind.RelativeOrAbsolute);
            AddResourceDictionary(resourceDictionary);
        }

        /// <summary>
        /// درج یک ریسورس
        /// </summary>
        /// <param name="address">آدرس ریسورس</param>
        public void AddResourceDictionary(object resourceDictionary)
        {
            ResourceDictionaries.Add((ResourceDictionary)resourceDictionary);
        }

        public bool ContineKey(string key)
        {
            foreach (var item in ResourceDictionaries)
            {
                if (item.Source != null && item.Source.ToString() == key)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// دریافت یک آیتم از لیست دیکشنری
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object GetItemFromList(object data)
        {
            foreach (var dic in ResourceDictionaries)
            {
                if (dic.Contains(data))
                    return dic[data];
            }
            return null;
        }

        /// <summary>
        /// دریافت یک ریسورس دیکشنری از لیست
        /// </summary>
        /// <param name="data">نام دیکشنری</param>
        /// <returns>دیکشنری مورد نظر</returns>
        public ResourceDictionary GetResourceDictionarieFromList(object data)
        {
            foreach (var dic in ResourceDictionaries)
            {
                if (dic.Contains(data))
                    return dic;
            }
            return null;
        }

        Dictionary<object, string> items = new Dictionary<object, string>();
        /// <summary>
        /// دریافت یک متن از ریسورس
        /// </summary>
        /// <param name="key">کلید ریسورس</param>
        /// <param name="nullable">آیا خالی برگرداند؟</param>
        /// <returns>متن مورد نظر</returns>
        public string GetAppResource(object key, bool nullable = false)
        {
            if (items.ContainsKey(key))
                return items[key];
            var obj = GetItemFromList(key);

            if (obj != null)
            {
                items.Add(key, obj.ToString());
                return items[key];
            }
            if (nullable)
                return "";
            return "Not found key from application resources!";
        }

        /// <summary>
        /// دریافت یک آبجکت با کلید
        /// </summary>
        /// <param name="key">کلید مورد نظر</param>
        /// <returns>آبجکت مورد نظر</returns>
        public object GetAppResourceObject(object key)
        {
            return GetItemFromList(key);
        }

        /// <summary>
        /// دریافت یک استایل
        /// </summary>
        /// <param name="key">کلید مورد نظر</param>
        /// <returns>استایل مورد نظر</returns>
        public Style GetAppResourceStyle(object key)
        {
            var obj = GetItemFromList(key);
            if (obj is Style)
                return (Style)obj;
            return null;
        }

        /// <summary>
        /// دریافت نوع داده ای خاص
        /// </summary>
        /// <typeparam name="T">نوع داده ی کلاس</typeparam>
        /// <param name="key">کلید مورد نظر</param>
        /// <returns>خروجی نوع کلاس مورد نظر</returns>
        public T GetAppResource<T>(object key) where T : class
        {
            var obj = GetItemFromList(key);
            if (obj is T)
                return (T)obj;
            return null;
        }

        /// <summary>
        /// تغییر یک ریسورس
        /// </summary>
        /// <param name="name">نام</param>
        /// <param name="newValue">داده ی جدید</param>
        public void SetAppResource(string name, object newValue)
        {
            var dic = GetResourceDictionarieFromList(name);
            dic[name] = newValue;
        }

        /// <summary>
        /// خالی کردن حافظه
        /// </summary>
        public void CollectMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


        public void EnterDispatcherThreadActionBeginRenderMode(Action action)
        {
            var Dispatcher = ((System.Windows.Threading.Dispatcher)DispatcherThread);
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "EnterDispatcherThreadActionBeginRenderMode");
                }
            }));
        }

    }
}
