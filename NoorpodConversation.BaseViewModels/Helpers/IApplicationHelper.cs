using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.BaseViewModels.Helpers
{
    /// <summary>
    /// کلاس داده های پیشفرض یک نرم افزار
    /// </summary>
    public interface IApplicationHelper
    {
        Action RefreshCommandsAction { get; set; }
        /// <summary>
        /// مسیر اجرای نرم افزار
        /// </summary>
        string ApplicationPath { get; }
        /// <summary>
        /// زبان نرم افزار
        /// </summary>
        string CurrentLanguage { get; set; }
        /// <summary>
        /// دیپسچر ترد برای مدیریت مالتی ترد ها
        /// </summary>
        object DispatcherThread { get; set; }

        /// <summary>
        /// ارسال تغییرات روی رابط کاربری برای لیست
        /// </summary>
        /// <param name="action">اکشن اعمال تغییرات</param>
        void EnterDispatcherThreadActionForCollections(Action action, object dispatcher);
        /// <summary>
        /// اعمال تغییرات از یک ترد روی رابط کاربری
        /// </summary>
        /// <param name="action">اکشن تغییرات</param>
        void EnterDispatcherThreadActionBegin(Action action);
        /// <summary>
        /// اعمال تغییرات از یک ترد روی رابط کاربری بعد از رندر
        /// </summary>
        /// <param name="action">اکشن تغییرات</param>
        void EnterDispatcherThreadActionBeginRenderMode(Action action);
        /// <summary>
        /// اعمال تغییرات از یک ترد روی رابط کاربری
        /// </summary>
        /// <param name="action">اکشن تغییرات</param>
        void EnterDispatcherThreadAction(Action action);

        /// <summary>
        /// اعمال تغییرات از یک ترد روی رابط کاربری
        /// </summary>
        /// <param name="action">اکشن تغییرات</param>
        void EnterDispatcherThreadFromCustomDispatcher(Action action, object dispatcher);

        /// <summary>
        /// درج یک ریورس دیکشنری توی حافظه
        /// </summary>
        /// <param name="address"></param>
        void AddResourceDictionary(string address);
        /// <summary>
        /// درج یک ریورس دیکشنری توی حافظه
        /// </summary>
        void AddResourceDictionary(object resourceDictionary);
        /// <summary>
        /// دریافت یک متن از ریورس
        /// </summary>
        /// <param name="key">کلید مورد نظر</param>
        /// <param name="nullable">آیا نال هم برگرداند اگر وجود نداشت؟</param>
        /// <returns>متن مورد نظر</returns>
        string GetAppResource(object key, bool nullable = false);
        /// <summary>
        /// دریافت یک آبجکت از ریسورس
        /// </summary>
        /// <param name="key">کلید مورد نظر</param>
        /// <returns>آبجکت مورد نظر</returns>
        object GetAppResourceObject(object key);
        /// <summary>
        /// تغییر یک ریسورس
        /// </summary>
        /// <param name="name">نام</param>
        /// <param name="newValue">مقدار جدید</param>
        void SetAppResource(string name, object newValue);
        /// <summary>
        /// دریافت یک نوع داده ای خاص از ریسورس
        /// </summary>
        /// <typeparam name="T">نوع</typeparam>
        /// <param name="key">کلید</param>
        /// <returns>مقدار داده</returns>
        T GetAppResource<T>(object key) where T : class;
        /// <summary>
        /// خالی کردن حافظه ی غیر قابل استفاده
        /// </summary>
        void CollectMemory();
        /// <summary>
        /// انجام عملیات رندرینگ با سرعت بالاتر
        /// </summary>
        /// <param name="action">اکشن شما که در ان باید بارگزاری کنید</param>
        void SuspendUI(Action action);
    }
}
