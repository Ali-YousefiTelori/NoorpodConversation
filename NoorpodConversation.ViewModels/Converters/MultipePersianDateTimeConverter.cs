using NoorpodConversation.BaseViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    public class ITwoDateTimePersianDateTimeConverter : IValueConverter
    {
        public PersianDateTimeConverter PersianConverter = new PersianDateTimeConverter();

        public object Convert(object values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null)
            {
                ITwoDateTime value = values as ITwoDateTime;
                if (parameter.ToString() == "0" || parameter.ToString() == "StartDatetime")
                    return PersianConverter.Convert(value.StartDatetime, null, null, null);
                else if (parameter.ToString() == "1" || parameter.ToString() == "EndDatetime")
                    return PersianConverter.Convert(value.EndDatetime, null, null, null);
            }

            return "not? why?";
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}