using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// تبدیل یک بازه ی زمانی به روز
    /// </summary>
    public class TimeSpanToDaysConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is TimeSpan)
                return ((TimeSpan)value).Days < 0 ? 0 : ((TimeSpan)value).Days;
            return 0;
        }
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}