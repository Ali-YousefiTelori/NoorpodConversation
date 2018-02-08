using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NoorpodConversation
{
    partial class NoorpodsService : ServiceBase
    {
        public NoorpodsService()
        {
            //InitializeComponent();
            ServiceName = "NoorpodsWindowsService";
        }


        protected override void OnStart(string[] args)
        {
          
        }

       
        protected override void OnStop()
        {
            AutoLogger.LogText("NoorpodsService OnStop");
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
