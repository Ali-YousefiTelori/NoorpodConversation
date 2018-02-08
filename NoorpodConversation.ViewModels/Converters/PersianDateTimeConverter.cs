using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// تبدیل یک تاریخ میلادی به شمسی
    /// </summary>
    public class PersianDateTimeConverter : IValueConverter
    {
        /// <summary>
        /// تبدیلگر
        /// </summary>
        static Calendar cal = new PersianCalendar();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime?)
            {
                if (((DateTime?)value).HasValue)
                    value = ((DateTime?)value).Value;
                else
                    return "...";
            }
            if (value is long)
                value = new DateTime((long)value);
            else if (value is decimal)
            {
                value = new DateTime((long)(decimal)value);
            }
            if (value is DateTime)
            {
                if ((DateTime)value == DateTime.MinValue) return 0;
                var dt = (DateTime)value;
                if (OnlyDay)
                    return (GetWeekDay(dt) + cal.GetDayOfMonth(dt) + GetMonthName(dt) + GetYear(dt)).Trim();
                var text = cal.GetYear(dt).ToString().Substring(2, 2) + "/" + cal.GetMonth(dt) + "/" + cal.GetDayOfMonth(dt);
                return GetWeekDay(dt) + GetTime(dt) + text + GetMonthName(dt);
            }
            return "...";
        }

        string GetMonthName(DateTime dt)
        {
            if (!ShowMonthName)
                return "";
            var month = cal.GetMonth(dt);
            return " " + ApplicationHelper.Current.GetAppResource(month + "Month_Persian") + " ";
        }

        string GetWeekDay(DateTime dt)
        {
            if (!ShowWeekDay)
                return "";
            var dayOfWeek = cal.GetDayOfWeek(dt);
            return ApplicationHelper.Current.GetAppResource(dayOfWeek + "S_Persian") + " ";
        }

        string GetTime(DateTime dt)
        {
            if (!IsShowTime)
                return "";
            var dayOfWeek = cal.GetDayOfWeek(dt);
            return " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + " ";
        }

        string GetYear(DateTime dt)
        {
            if (!IsShowYear)
                return "";
            return cal.GetYear(dt).ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }

        /// <summary>
        /// فقط نمایش روز
        /// </summary>
        public bool OnlyDay { get; set; }

        /// <summary>
        /// نمایش نام ماه
        /// </summary>
        public bool ShowMonthName { get; set; }

        /// <summary>
        /// نمایش روز هفته
        /// </summary>
        public bool ShowWeekDay { get; set; }
        /// <summary>
        /// نمایش سال
        /// </summary>
        public bool IsShowYear { get; set; }
        /// <summary>
        /// نمایش زمان
        /// </summary>
        public bool IsShowTime { get; set; }
    }
}