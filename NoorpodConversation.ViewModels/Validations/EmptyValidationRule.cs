using NoorpodConversation.BaseViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace NoorpodConversation.ViewModels.Validations
{
    public class EmptyValidationRule : ValidationCheckRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (ApplicationHelper.Current == null)
                return new ValidationResult(true, null);

            value = GetBoundValue(value);
            if (value == null || string.IsNullOrEmpty(value.ToString().Trim()))
            {
                HasError = true;
                return new ValidationResult(false, GetCustomError(ApplicationHelper.Current.GetAppResource("MustSetValue_Language")));
            }
            HasError = false;
            return new ValidationResult(true, null);
        }
    }
}
