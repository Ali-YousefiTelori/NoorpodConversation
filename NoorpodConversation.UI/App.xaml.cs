using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NoorpodConversation.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class App : Application, ISingleInstanceApp
    {
        private const string Unique = "Atitec Civil Management";
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

            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
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
                SingleInstance<App>.Cleanup();
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
            using (Stream stream = a.GetManifestResourceStream("CivilManagement.MergedResources.xaml"))
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

        static void SelectivelyIgnoreMouseButton(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = sender as UIElement;
            //while (parent != null && !(parent is System.Windows.Controls.TextBox))
            //{
            //    if (parent is AtitecCalendar.MonthlyCalendar)
            //        return;
            //    parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            //}
            if (parent != null && parent is TextBox && e.OriginalSource.GetType().Name == "TextBoxView")
            {
                var textBox = (System.Windows.Controls.TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focused, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as System.Windows.Controls.TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

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

            // Select the text in a TextBox when it receives focus.
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.PreviewMouseLeftButtonDownEvent,
                new System.Windows.Input.MouseButtonEventHandler(SelectivelyIgnoreMouseButton));
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.GotKeyboardFocusEvent,
                new RoutedEventHandler(SelectAllText));
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.MouseDoubleClickEvent,
                new RoutedEventHandler(SelectAllText));

            ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(0));

            ManualResetEvent resetEvent = new ManualResetEvent(false);
            LoadingWindow loadingWindow = null;
            //object lockOBJ = new object();
            //bool loaded = false;
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    loadingWindow = new LoadingWindow();
                    loadingWindow.Show();
                    loadingWindow.Activate();
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "OnStartup LoadingWindow");
                }
                finally
                {
                    resetEvent.Set();
                }
                // Start the Dispatcher Processing
                System.Windows.Threading.Dispatcher.Run();
            }));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            // Start the thread
            newWindowThread.Start();
            try
            {
                MainWindow windows = new MainWindow();
                windows.Dispatcher.UnhandledException += application.Dispatcher_UnhandledException;
                windows.Dispatcher.UnhandledExceptionFilter += application.Dispatcher_UnhandledExceptionFilter;

                windows.Show();
                CivilManagement.Views.Busy.BusyWindow.GenerateOfWindow(windows);
                loadingWindow.Dispatcher.Invoke(new Action(() =>
                {
                    loadingWindow.Close();
                }));
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
