using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// تبدیل چند عدد به یک Thichness
    /// </summary>
    public class IntToThicknessMultipeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double left = 0.0, top = 0.0, right = 0.0, bottom = 0.0;
            for (int i = 0; i < values.Length; i++)
            {
                if (i == 0)
                {
                    if (values[i] != null)
                        double.TryParse(values[i].ToString(), out left);
                }
                if (i == 1)
                {
                    if (values[i] != null)
                        double.TryParse(values[i].ToString(), out top);
                }
                if (i == 2)
                {
                    if (values[i] != null)
                        double.TryParse(values[i].ToString(), out right);
                }
                if (i == 3)
                {
                    if (values[i] != null)
                        double.TryParse(values[i].ToString(), out bottom);
                }
            }
            return new Thickness(left, top, right, bottom);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
