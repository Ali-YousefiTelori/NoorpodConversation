using NoorpodConversation.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    public class IsValidationWarningConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var errors = values[1] as IEnumerable<ValidationError>;
            if (errors == null)
                return false;
            bool isWarning = false;
            foreach (var item in errors)
            {
                if (item.RuleInError is ValidationCheckRule)
                {
                    var rule = item.RuleInError as ValidationCheckRule;
                    if (!rule.IsWarning)
                        return false;
                    else
                        isWarning = true;
                }
            }
            return isWarning;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //public class IsValidationWarningConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var errors = value as IEnumerable<ValidationError>;
    //        bool isWarning = false;
    //        foreach (var item in errors)
    //        {
    //            if (item.RuleInError is ValidationCheckRule)
    //            {
    //                var rule = item.RuleInError as ValidationCheckRule;
    //                if (!rule.IsWarning)
    //                    return false;
    //                else
    //                    isWarning = true;
    //            }
    //        }
    //        return isWarning;
    //    }

    //    public object ConvertBack(object value, Type targetType,
    //        object parameter, CultureInfo culture)
    //    {
    //        return value;
    //    }
    //}
}