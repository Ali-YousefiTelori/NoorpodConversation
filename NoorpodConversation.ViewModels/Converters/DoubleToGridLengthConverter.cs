using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// تبدیل یک عدد دابل به اندازه ی قابل شناسایی توسط گرید
    /// </summary>
    public class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = 0.0;
            if (value is double)
            {
                val = (double)value;
            }
            else
                double.TryParse(value.ToString(), out val);
            GridLength len = new GridLength(val, GridUnitType.Star);
            return len;
        }
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}