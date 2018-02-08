using NoorpodConversation.BaseViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace NoorpodConversation.ViewModels.Validations
{
    public class IntValidationRule : ValidationCheckRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            value = GetBoundValue(value);
            int result;
            //int test;
            if (value != null && int.TryParse(value.ToString(), out result) && int.TryParse(value.ToString(), out result))
            {
                //result = Agrin_Engine.Models.Math.Convertor.ConvertToByte(result, (SizeEnum)(ValidationMode + VisibleCount), SizeEnum.Byte);
                if (result > MaxValue || result > MaxValue)
                {
                    if (MaxValue != int.MaxValue)
                    {
                        HasError = true;
                        return new ValidationResult(false, GetCustomError(ApplicationHelper.Current.GetAppResource("MinValidation_Language") + " " + MaxValue + " " + ApplicationHelper.Current.GetAppResource("IsValidation_Language")));
                    }
                    else
                    {
                        HasError = true;
                        return new ValidationResult(false, GetCustomError(ApplicationHelper.Current.GetAppResource("IsMaxValidation_Language")));
                    }
                }
                else if (result < MinValue || result < MinValue)
                {
                    HasError = true;
                    return new ValidationResult(false, GetCustomError(ApplicationHelper.Current.GetAppResource("MaxValidation_Language") + " " + MinValue + " " + ApplicationHelper.Current.GetAppResource("IsValidation_Language")));
                }
                HasError = false;
                return new ValidationResult(true, null);
            }
            else
            {
                HasError = true;
                return new ValidationResult(false, ApplicationHelper.Current.GetAppResource("PleaseSetTrue_Language"));
            }
        }

        int _maxValue = int.MaxValue;

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

        int _validationMode;

        public int ValidationMode
        {
            get { return _validationMode; }
            set { _validationMode = value; }
        }

        int _visibleCount;

        public int VisibleCount
        {
            get { return _visibleCount; }
            set { _visibleCount = value; }
        }
    }
}
