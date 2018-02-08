using Atitec.BaseViewModels.ComponentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AtitecInforming.Models
{
    public enum InformingState
    {
        Loading,
        Success,
        Fail,
    }

    public class InformingMessage : ANotifyPropertyChanged
    {
        public InformingMessage()
        {
            IgnoreStopChanged = true;
        }

        string _Title;
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
        public Window MainWindow { get; set; }
        public Action IsShowedChangedAction { get; set; }
    }
}
