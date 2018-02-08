using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.BaseViewModels.Validations
{
    public interface IValidationRule
    {
        Action<IValidationRule> Updated { get; set; }
        Func<object, object, string> GetCustomValidationMessageFunction { get; set; }

        bool HasError { get; set; }
        bool IsWarning { get; set; }
        int ValidationGroup { get; set; }
        string ChangedPropertyName { get; set; }
        object DataContext { get; set; }

        void ManualValidate(object value);
    }
}
