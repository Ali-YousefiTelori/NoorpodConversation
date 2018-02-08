using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NoorpodConversation.BaseViewModels.Network
{
    public static class Internet
    {
        public static event Action<bool> ChangedInternetConnectionAction;

        static bool _HasInternetConnection;

        public static bool HasInternetConnection
        {
            get { return Internet._HasInternetConnection; }
            set
            {
                if (_HasInternetConnection != value)
                {
                    if (ChangedInternetConnectionAction != null)
                        ChangedInternetConnectionAction(value);
                }
                Internet._HasInternetConnection = value;
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                var web = WebRequest.Create("http://www.google.com");
                web.Timeout = 10 * 1000;
                web.Proxy = null;
                using (var stream = web.GetResponse())
                {
                    web.Abort();
                    HasInternetConnection = true;
                }
            }
            catch
            {
                HasInternetConnection = false;
            }
            return HasInternetConnection;
        }

        static bool _started = false;
        public static void StartChecker()
        {
            if (_started)
                return;
            _started = true;
            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
            {
                while(true)
                {
                    CheckForInternetConnection();
                    System.Threading.Thread.Sleep(10 * 1000);
                }
            });

            task.Start();
        }
    }
}