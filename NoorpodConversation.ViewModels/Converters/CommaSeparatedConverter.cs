using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    public class CommaSeparatedConverter : IValueConverter
    {
        public static string ConvertToCommaSeparated(string val)
        {
            val = new string(val.Reverse().ToArray());
            string conveter = "";
            int i = 1;
            foreach (var item in val)
            {
                conveter += item;
                if (i == 3)
                {
                    conveter += ",";
                    i = 1;
                }
                else
                    i++;
            }
            conveter = new string(conveter.Reverse().ToArray());
            return conveter.Trim(new char[] { ',' });
        }
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string val = value.ToString();
            return ConvertToCommaSeparated(val);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value.ToString().Replace(",", "");
        }
    }
}
