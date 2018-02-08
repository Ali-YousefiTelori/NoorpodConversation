using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// تبدیل گر اینکه ایا اولین روز ماه است یا خیر
    /// </summary>
    public class IsFirstDayOfMonthConverter : IValueConverter
    {
        Calendar cal = new PersianCalendar();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime)value;
            if (dateTime == null)
                return false;
            return IsPersian ? cal.GetDayOfMonth(dateTime) == 1 : dateTime.Day == 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        bool _IsPersian = true;

        public bool IsPersian
        {
            get { return _IsPersian; }
            set { _IsPersian = value; }
        }
    }
}