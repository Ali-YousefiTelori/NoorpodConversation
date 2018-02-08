using NoorpodConversation.Callbacks;
using NoorpodConversation.DataBase.Context;
using NoorpodConversation.Services;
using SignalGo.Server.ServiceManager;
using SignalGo.Shared;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoorpodConversation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("service is starting...");
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);
            //sadsd
            try
            {
                Console.WriteLine("generating database...");
                using (NoorpodContext context = new NoorpodContext())
                {
                    var users = context.Users.Count();
                    if (users == 0)
                    {
                        context.Users.Add(new DataBase.Models.UserInfo() { IsAdmin = true, Password = "123", UserName = "admin" });
                        context.SaveChanges();
                        users++;
                    }
                    Console.WriteLine("users count: " + users);
                }


                //AsyncActions.OnActionException = (ex) =>
                //{
                //    Console.WriteLine(ex, "AsyncActions.OnActionException");
                //};
                ServerProvider provider = new ServerProvider();

                provider.Start("http://localhost:6987/Noorpods/ConversationService");
                provider.InitializeService(typeof(ConversationService));
                provider.RegisterClientCallbackInterfaceService<IConversationCallback>();
                provider.ConnectToUDP(13020);
                Console.WriteLine("service started successfuly");
            }
            catch (Exception ex)
            {
                AutoLogger.LogText("error");
                AutoLogger.LogError(ex, "Main");
                Console.WriteLine("Error");
                Console.WriteLine(ex);
            }
            Console.ReadKey();
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
