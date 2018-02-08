using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    public enum DateTimeModeEnum
    {
        Date,
        Time,
        All
    }

    public class NoorpodConversationDateTimeConverter : IValueConverter
    {
        public bool IsInverse { get; set; }
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string[] dt = value.ToString().Split(' ');
            if (Mode == DateTimeModeEnum.Date)
                return dt[0];
            else if (Mode == DateTimeModeEnum.Time)
                return dt[1];
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }

        public DateTimeModeEnum Mode { get; set; } = DateTimeModeEnum.All;
    }
}