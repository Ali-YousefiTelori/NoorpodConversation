using NoorpodConversation.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// تبدیل سطر ها به ایندکس
    /// </summary>
    public class ListBoxItemToRowIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListBoxItem listBoxItem = value as ListBoxItem;
            var listBox = listBoxItem.Parent as ListBox;
            int index = 0;
            if (listBox == null)
            {
                listBox = ViewsUtility.FindParent<ListBox>(listBoxItem);
                index = listBox.Items.IndexOf(listBoxItem.DataContext);
            }
            else
                index = listBox.Items.IndexOf(listBoxItem);
            var row = index % 7;
            return row;
        }


        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}