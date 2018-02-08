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
    public class ValidationErrorsToFirstErrorContentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var errors = values[1] as IEnumerable<ValidationError>;
            if (errors == null)
                return "نامشخص";
            ValidationError lastWarning = null;
            foreach (var item in errors)
            {
                if (item.RuleInError is ValidationCheckRule)
                {
                    var rule = item.RuleInError as ValidationCheckRule;
                    if (!rule.IsWarning)
                        return item.ErrorContent;
                    else
                        lastWarning = item;
                }
            }
            if (lastWarning != null)
                return lastWarning.ErrorContent;
            return errors.Count() > 0 ? errors.First().ErrorContent : "نامشخص";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}