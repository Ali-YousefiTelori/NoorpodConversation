using Microsoft.Owin.Hosting;
using NoorpodConversation.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoorpodConversation.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        StreamingSocketServer socket = new StreamingSocketServer();
        public static string LastError = "not value set";
        protected void Application_Start()
        {
            try
            {
                LastError = "starting";
                AreaRegistration.RegisterAllAreas();
                string url = @"http://+:6987/Services/AudioConversation/SignalR";
                WebApp.Start<NoorpodSignalRStartup>(url);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                LastError = "hub ok";
                socket.Start();
                LastError += "AllSuccess";
            }
            catch(Exception ex)
            {
                LastError += ex.Message;
            }
        }
    }
}
