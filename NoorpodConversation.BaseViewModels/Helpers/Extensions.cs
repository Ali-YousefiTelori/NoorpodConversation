using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System
{
    /// <summary>
    /// اکستنشن های کاربردی
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// تایپ های عددی سی شارپ
        /// </summary>
        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),  
            typeof(uint), typeof(float)
        };

        /// <summary>
        /// یک تایپ میگیرد و نشان میدهد که تایپش عددی هست یا نه
        /// </summary>
        /// <param name="myType"></param>
        /// <returns></returns>
        public static bool IsNumeric(this Type myType)
        {
            return NumericTypes.Contains(Nullable.GetUnderlyingType(myType) ?? myType);
        }

        /// <summary>
        /// یک لیست میگیرد و یک لیست از همان نوع منتهی برعکس شده میدهد
        /// </summary>
        /// <typeparam name="T">نوع مورد نظر</typeparam>
        /// <param name="source">لیست مورد نظر</param>
        /// <returns>خروجی لیست برعکس شده</returns>
        public static IEnumerable<T> Invert<T>(this IEnumerable<T> source)
        {
            var transform = source.Select(
                (o, i) => new
                {
                    Index = i,
                    Object = o
                });

            return transform.OrderByDescending(o => o.Index)
                            .Select(o => o.Object);
        }

        public static DateTime GetMonthDate(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        static PersianCalendar cal = new PersianCalendar();
        public static string GetPersianMonthDate(this DateTime dt)
        {
            return cal.GetYear(dt) + "/" + cal.GetMonth(dt);
        }
    }
}
