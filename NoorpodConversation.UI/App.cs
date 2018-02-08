using Microsoft.Shell;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;

namespace NoorpodConversation.UI
{
#pragma warning disable CS0436 // Type conflicts with imported type
                              /// <summary>
                              /// Interaction logic for App.xaml
                              /// </summary>
    public class App : Application, ISingleInstanceApp
#pragma warning restore CS0436 // Type conflicts with imported type
    {
        private const string Unique = "Noorpod Conversation";
        //public static bool ForceSoftwareRendering
        //{
        //    get
        //    {
        //        int renderingTier = (System.Windows.Media.RenderCapability.Tier >> 16);
        //        return renderingTier == 0;
        //    }
        //}
        [STAThread]
        public static void Main()
        {

#pragma warning disable CS0436 // Type conflicts with imported type
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
#pragma warning restore CS0436 // Type conflicts with imported type
            {
                //if (ForceSoftwareRendering)
                //{
                //System.Windows.Media.RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
                //}
                var application = new App();
                application.Startup += OnStartup;
                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
#pragma warning disable CS0436 // Type conflicts with imported type
                SingleInstance<App>.Cleanup();
#pragma warning restore CS0436 // Type conflicts with imported type
            }
        }

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;

            this.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
            Assembly a = Assembly.GetExecutingAssembly();
            using (Stream stream = a.GetManifestResourceStream("NoorpodConversation.UI.MergedResources.xaml"))
            {
                XmlReader XmlRead = XmlReader.Create(stream);
                Application.Current.Resources = (ResourceDictionary)XamlReader.Load(XmlRead);
                XmlRead.Close();
            }
        }
        #region ISingleInstanceApp Members

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            return true;
        }

        #endregion
        /// <summary>
        ///   Raises the <see cref = "Application.Startup" /> event.
        /// </summary>
        /// <param name = "e">The <see cref = "System.Windows.StartupEventArgs" /> instance containing the event data.</param>
        /// <param name = "isFirstInstance">If set to <c>true</c> the current instance is the first application instance.</param>
        static void OnStartup(object sender, StartupEventArgs e)
        {
            App application = sender as App;

            application.DispatcherUnhandledException += application.App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += application.CurrentDomain_UnhandledException;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += application.TaskScheduler_UnobservedTaskException;
            Application.Current.DispatcherUnhandledException += application.Current_DispatcherUnhandledException;

           
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            //LoadingWindow loadingWindow = null;
            ////object lockOBJ = new object();
            ////bool loaded = false;
            //Thread newWindowThread = new Thread(new ThreadStart(() =>
            //{
            //    try
            //    {
            //        loadingWindow = new LoadingWindow();
            //        loadingWindow.Show();
            //        loadingWindow.Activate();
            //    }
            //    catch (Exception ex)
            //    {
            //        AutoLogger.LogError(ex, "OnStartup LoadingWindow");
            //    }
            //    finally
            //    {
            //        resetEvent.Set();
            //    }
            //    // Start the Dispatcher Processing
            //    System.Windows.Threading.Dispatcher.Run();
            //}));
            //newWindowThread.SetApartmentState(ApartmentState.STA);
            //// Make the thread a background thread
            //newWindowThread.IsBackground = true;
            //// Start the thread
            //newWindowThread.Start();
            try
            {
                MainWindow windows = new MainWindow();
                windows.Dispatcher.UnhandledException += application.Dispatcher_UnhandledException;
                windows.Dispatcher.UnhandledExceptionFilter += application.Dispatcher_UnhandledExceptionFilter;

                windows.Show();
                //CivilManagement.Views.Busy.BusyWindow.GenerateOfWindow(windows);
                //loadingWindow.Dispatcher.Invoke(new Action(() =>
                //{
                //    loadingWindow.Close();
                //}));
                System.Windows.Threading.Dispatcher.Run();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                AutoLogger.LogError(ex, "App Run");
            }
        }

        void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            AutoLogger.LogError(e.Exception, "Unhandled Exception 1");
        }

        void TaskScheduler_UnobservedTaskException(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            AutoLogger.LogError(e.Exception, "TaskScheduler Unhandled Exception");
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            AutoLogger.LogError(e.Exception, "TaskScheduler Unhandled Exception 2");
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
                AutoLogger.LogError((Exception)e.ExceptionObject, "Domain Unhandled Exception");
            else if (e.ExceptionObject == null)
            {
                AutoLogger.LogText("Domain Unhandled Exception Null");
            }
            else
            {
                AutoLogger.LogText("Type Domain Unhandled Exception :" + e.ExceptionObject.ToString());
            }
        }

        void Dispatcher_UnhandledExceptionFilter(object sender, System.Windows.Threading.DispatcherUnhandledExceptionFilterEventArgs e)
        {
            e.RequestCatch = true;
            AutoLogger.LogError(e.Exception, "Unhandled Exception Filter");
        }

        void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            AutoLogger.LogError(e.Exception, "Unhandled Exception 3");
        }
    }
}
