using NoorpodConversation.BaseViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace NoorpodConversation.ViewModels.Validations
{
    public class LengthValidationRule : ValidationCheckRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (ApplicationHelper.Current == null)
                return new ValidationResult(true, null);

            value = GetBoundValue(value);
            if (String.IsNullOrEmpty(value as String))
            {
                HasError = true;
                return new ValidationResult(false, GetCustomError(ApplicationHelper.Current.GetAppResource("MustSetValue_Language")));
            }
            int lenght = value.ToString().Length;
            if (lenght > MaxValue)
            {
                HasError = true;
                return new ValidationResult(false, ApplicationHelper.Current.GetAppResource("MinCharValidation_Language") + " " + MaxValue + " " + ApplicationHelper.Current.GetAppResource("IsValidation_Language"));
            }
            else if (lenght < MinValue)
            {
                HasError = true;
                return new ValidationResult(false, GetCustomError(ApplicationHelper.Current.GetAppResource("MaxCharValidation_Language") + " " + MinValue + " " + ApplicationHelper.Current.GetAppResource("IsValidation_Language")));
            }
            else if (lenght < MinValue || lenght > MaxValue)
            {
                HasError = true;
                return new ValidationResult(false, GetCustomError(ApplicationHelper.Current.GetAppResource("MustCharValidation_Language") + " " + MinValue + " " + ApplicationHelper.Current.GetAppResource("IsRaghamValidation_Language")));
            }

            HasError = false;
            return new ValidationResult(true, null);
        }

        int _maxValue = 0;

        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        int _minValue = 0;

        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        bool _IsNullable = false;

        public bool IsNullable
        {
            get { return _IsNullable; }
            set
            {
                _IsNullable = value;
            }
        }
    }
}