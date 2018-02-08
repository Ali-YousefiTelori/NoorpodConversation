using System.Data.SqlClient;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Smo;

using NoorpodConversation.Callbacks;
using NoorpodConversation.DataBase.Context;
using NoorpodConversation.Services;
using SignalGo.Server.ServiceManager;
using SignalGo.Shared;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;

namespace NoorpodConversation.WindowsService
{
    static class Program
    {
        static ServerProvider provider = new ServerProvider();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);
            //sadsd
            try
            {
                using (NoorpodContext contxt = new NoorpodContext())
                {
                    AutoLogger.LogText("users count: " + contxt.Users.ToList().Count);
                }
               

                AsyncActions.OnActionException = (ex) =>
                {
                    AutoLogger.LogError(ex, "AsyncActions.OnActionException");
                };

                provider.Start("http://localhost:6987/Noorpods/ConversationService");
                provider.InitializeService(typeof(ConversationService));
                provider.RegisterClientCallbackInterfaceService<IConversationCallback>();
                provider.ConnectToUDP(13020);
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new NoorpodsService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                AutoLogger.LogText("error");
                AutoLogger.LogError(ex, "Main");
            }
        }

        private static void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex == null)
            {
                AutoLogger.LogError(ex, "NoorpodsService UnhandledException ex = null && IsTerminating :" + e.IsTerminating);
            }
            else
                AutoLogger.LogError(ex, "NoorpodsService UnhandledException && IsTerminating :" + e.IsTerminating);
        }

    }
}
