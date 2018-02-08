using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// این تبدیل گر تاری ورودی را تبدیل به شمسی کرده و بررسی میکند که روز جمعه هست یا خیر
    /// </summary>
    public class PersianDateTimeIsFridayConverter : IValueConverter
    {
        static Calendar cal = new PersianCalendar();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var dt = (DateTime)value;
                if (cal.GetDayOfWeek(dt) == DayOfWeek.Friday)
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}