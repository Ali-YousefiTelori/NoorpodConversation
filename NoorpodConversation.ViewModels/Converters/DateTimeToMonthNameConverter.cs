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
    /// تبدیل یک تاریخ به نام ماه آن تاریخ
    /// </summary>
    public class DateTimeToMonthNameConverter : IValueConverter
    {
        Calendar cal = new PersianCalendar();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime)value;
            if (dateTime == null)
                return "نامشخص";
            var month = cal.GetMonth(dateTime);
            return ApplicationHelper.Current.GetAppResource(month + (IsPersian ? "Month_Persian" : "Month_English")) + " " + (IsPersian ? cal.GetYear(dateTime).ToString().Substring(2, 2) : dateTime.Year.ToString());
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
