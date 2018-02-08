using System.Diagnostics;
using System.Windows.Input;

namespace System
{
    /// <summary>
    /// کامند قابل استفاده برای بایند کردن کنترل ها در WPF
    /// </summary>
    /// <typeparam name="T">نوع کلاس</typeparam>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// وقتی نیازمند این باشیم که یکبار این اکشن هنگام فراخوانی کامد اجرا شود
        /// </summary>
        public Action<T> ExecuteAction { get; set; }

        #region Fields

        /// <summary>
        /// اکشن قابل اجرا
        /// </summary>
        readonly Action<T> _execute = null;
        /// <summary>
        /// آیا اکشن میتواند اجرا شود؟
        /// </summary>
        readonly Predicate<T> _canExecute = null;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {

        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        /// <summary>
        /// آیا کامند قابلیت اجرا دارد یا خیر؟
        /// </summary>
        /// <param name="parameter">نوع داده</param>
        /// <returns>قابلیت اجرای کامند</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        /// <summary>
        /// وقتی نیاز شد که بررسی کند که کامند قابلیت اجرا دارد یا خیر
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// اجرای کامند
        /// </summary>
        /// <param name="parameter">داده ی مورد نظر</param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
            if (ExecuteAction != null)
                ExecuteAction((T)parameter);
        }

        #endregion // ICommand Members
    }

    /// <summary>
    /// کامند قابل استفاده برای بایند کردن کنترل ها در WPF 
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// وقتی نیازمند این باشیم که یکبار این اکشن هنگام فراخوانی کامد اجرا شود
        /// </summary>
        public Action ExecuteAction { get; set; }

        #region Fields

        /// <summary>
        /// اکشن قابل اجرا
        /// </summary>
        readonly Action _execute = null;
        /// <summary>
        /// آیا اکشن میتواند اجرا شود؟
        /// </summary>
        readonly Func<bool> _canExecute = null;
        private RelayCommand showSettingPage;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action execute)
            : this(execute, null)
        {

        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(RelayCommand showSettingPage)
        {
            this.showSettingPage = showSettingPage;
        }

        #endregion // Constructors

        #region ICommand Members
        /// <summary>
        /// آیا کامند قابلیت اجرا دارد یا خیر؟
        /// </summary>
        /// <param name="parameter">نوع داده</param>
        /// <returns>قابلیت اجرای کامند</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter = null)
        {
            return _canExecute == null ? true : _canExecute();
        }

        /// <summary>
        /// وقتی نیاز شد که بررسی کند که کامند قابلیت اجرا دارد یا خیر
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// اجرای کامند
        /// </summary>
        public void Execute()
        {
            Execute(null);
            if (ExecuteAction != null)
                ExecuteAction();
        }
        /// <summary>
        /// اجرای کامند
        /// </summary>
        /// <param name="parameter">دیتای مورد نظر</param>
        public void Execute(object parameter)
        {
            _execute();
            if (ExecuteAction != null)
                ExecuteAction();
        }

        #endregion // ICommand Members
    }
}
