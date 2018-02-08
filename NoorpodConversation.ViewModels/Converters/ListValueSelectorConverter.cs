using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    public class ValueItem
    {
        public object ID { get; set; }
        public object Value { get; set; }
    }

    public class ListValueSelectorConverter : IValueConverter
    {
        public ListValueSelectorConverter()
        {
            Items = new List<ValueItem>();
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            //if (!(value is int))
            //    return null;
            foreach (var item in Items)
            {
                //var type = value.GetType();
                //var type2 = item.ID.GetType();

                if (value.Equals(item.ID))
                    return item.Value;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }

        public List<ValueItem> Items { get; set; }
    }
}
