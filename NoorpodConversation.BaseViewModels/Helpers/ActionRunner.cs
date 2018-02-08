using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// بعد از مدتی اکشن مورد نظر را اجرا میکند
    /// </summary>
    public class TimeActionRunner
    {
        /// <summary>
        /// اکشن مورد نظر
        /// </summary>
        Action runAction = null;
        /// <summary>
        /// کانستراکتور کلاس
        /// </summary>
        /// <param name="run">اکشن جاری</param>
        public TimeActionRunner(Action run)
        {
            runAction = run;
        }
        /// <summary>
        /// اجرا کردن
        /// </summary>
        public void Start()
        {
            var task = new Task(() =>
            {
                System.Threading.Thread.Sleep(500);
                lock (lockOBJ)
                {
                    if (!_isDispose)
                        runAction();
                }
                _isDispose = true;
            });
            task.Start();
        }

        object lockOBJ = new object();

        bool _isDispose = false;
        /// <summary>
        /// حذف از حافظه
        /// </summary>
        public void Dispose()
        {
            lock (lockOBJ)
            {
                _isDispose = true;
            }
        }
    }

    /// <summary>
    /// اجرای اکشن در مواقع مورد نظر بدون وقفه
    /// </summary>
    public class NormalActionRunner
    {
        /// <summary>
        /// اکشن مورد نظر
        /// </summary>
        Action runAction = null;
        /// <summary>
        /// کانستراکتور کلاس
        /// </summary>
        /// <param name="run">اکشن جاری</param>
        public NormalActionRunner(Action run)
        {
            runAction = run;
        }

        /// <summary>
        /// اجرا
        /// </summary>
        public void Run()
        {
            if (!_isDispose)
                runAction();
        }

        object lockOBJ = new object();

        bool _isDispose = false;
        /// <summary>
        /// حذف از حافظه
        /// </summary>
        public void Dispose()
        {
            lock (lockOBJ)
            {
                _isDispose = true;
            }
        }
    }

    public static class AsyncRunner
    {
        public static void Run(Action run)
        {
            Task task = new Task(() =>
            {
               try
                {
                    run();
                }
                catch(Exception ex)
                {
                    AutoLogger.LogError(ex, "noopod AsyncRunner Run(");
                }
            });
            task.Start();
        }
    }
}
