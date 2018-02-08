using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System
{
    public static class InformingMessageExtension
    {
        public static Action<InformingMessage> ShowMessageAction { get; set; }
        public static Action<InformingMessage, string> FailAction { get; set; }
        public static Action<InformingMessage, string> SuccessAction { get; set; }

        public static InformingMessage Show(this InformingMessage informingMessage)
        {
            ShowMessageAction?.Invoke(informingMessage);
            return informingMessage;
        }

        public static void Sucess(this InformingMessage informingMessage, string title)
        {
            SuccessAction?.Invoke(informingMessage, title);
        }

        public static void Fail(this InformingMessage informingMessage, string title)
        {
            FailAction?.Invoke(informingMessage, title);
        }
    }

    public enum InformingState
    {
        Loading,
        Success,
        Fail,
    }

    public class InformingMessage : INotifyPropertyChanged
    {
        public static bool CanShowMessage { get; set; } = true;
        public static bool IsInformingEnabled { get; set; } = true;

        public static InformingMessage InstanceLoading(string title, string message, bool autoShow = true, object dispatcher = null)
        {
            if (!IsInformingEnabled || !CanShowMessage)
                return null;
            var msg = new InformingMessage(title, message) { Dispatcher = dispatcher };
            if (autoShow)
                msg.Show();
            return msg;
        }

        public static InformingMessage InstanceComeplete(string title, string message, bool autoShow = true, object dispatcher = null)
        {
            if (!IsInformingEnabled || !CanShowMessage)
                return null;
            var msg = new InformingMessage(title, message) { Dispatcher = dispatcher };
            if (autoShow)
                msg.Show();
            return msg;
        }

        public InformingMessage(string title, string message)
        {
            Status = InformingState.Loading;
            Title = title;
            Message = message;
        }

        string _Title;
        string _Message;
        string _ErrorMessaage;
        InformingState _Status;
        bool _IsShowed = false;
        bool _IsDisposed = false;

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                OnPropertyChanged("Message");
            }
        }

        public string ErrorMessaage
        {
            get
            {
                return _ErrorMessaage;
            }
            set
            {
                _ErrorMessaage = value;
                OnPropertyChanged("ErrorMessaage");
            }
        }

        public InformingState Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
                OnPropertyChanged("CanProgressActive");
            }
        }

        public bool CanProgressActive
        {
            get
            {
                return Status == InformingState.Loading;
            }
        }
        public bool IsShowed
        {
            get
            {
                return _IsShowed;
            }
            set
            {
                _IsShowed = value;
                OnPropertyChanged("IsShowed");
                IsShowedChangedAction?.Invoke();
            }
        }

        public bool IsDisposed
        {
            get
            {
                return _IsDisposed;
            }
            set
            {
                _IsDisposed = value;
                IsShowedChangedAction?.Invoke();
            }
        }

        public object Data { get; set; }
        public Action RetryAction { get; set; }
        public object MainWindow { get; set; }
        public Action IsShowedChangedAction { get; set; }

        public object Dispatcher { get; set; }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
