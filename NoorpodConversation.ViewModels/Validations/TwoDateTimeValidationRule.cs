using NoorpodConversation.BaseViewModels.Helpers;
using NoorpodConversation.BaseViewModels.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace NoorpodConversation.ViewModels.Validations
{
    public class TwoDateTimeValidationRule : ValidationCheckRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            value = GetBoundValue(value);
            ITwoDateTime twoDateTime = value as ITwoDateTime;
            if (twoDateTime == null || twoDateTime.EndDatetime.Date <= twoDateTime.StartDatetime.Date)
            {
                HasError = true;
                return new ValidationResult(false, GetCustomError("تاریخ شروع باید کوچکتر از تاریخ پایان باشد"));
            }
            HasError = false;
            return new ValidationResult(true, null);
        }
    }
}
