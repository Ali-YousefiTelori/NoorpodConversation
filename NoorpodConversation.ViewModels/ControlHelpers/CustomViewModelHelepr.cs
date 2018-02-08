using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NoorpodConversation.ViewModels.ControlHelpers
{
    public class CustomViewModelHelepr
    {
        public Action<FrameworkElement> ControlDataChanged { get; set; }
        FrameworkElement _ControlData;
        public FrameworkElement ControlData
        {
            get
            {
                return _ControlData;
            }
            set
            {
                _ControlData = value;
                ControlDataChanged?.Invoke(value);
            }
        }
    }
}
