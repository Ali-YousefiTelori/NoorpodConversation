using NoorpodConversation.BaseViewModels.Validations;
using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Validations
{
    public abstract class ValidationCheckRule : ValidationRule, IValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return null;
        }

        bool updatedSource = false;

        public object GetBoundValue(object value)
        {
            if (value is BindingExpression)
            {
                try
                {
                    // ValidationStep was UpdatedValue or CommittedValue (validate after setting)
                    // Need to pull the value out of the BindingExpression.
                    BindingExpression binding = (BindingExpression)value;

                    // Get the bound object and name of the property
                    string resolvedPropertyName = binding.ParentBinding.Path.Path;
                    object resolvedSource = binding.DataItem;

                    // Extract the value of the property
                    object propertyValue = resolvedSource.GetType().GetProperty(resolvedPropertyName).GetValue(resolvedSource, null);

                    return propertyValue;
                }
                catch (Exception ex)
                {
                    AutoLogger.LogError(ex, "ValidationCheckRule GetBoundValue");
                    return value;
                }
            }
            else
            {
                return value;
            }
        }

        public string GetCustomError(string newEx)
        {
            if (String.IsNullOrEmpty(CustomContent))
                return newEx;
            return CustomContent;
        }

        public void ManualValidate(object value)
        {
            if (CanManualValidate)
                Validate(value, null);
        }

        bool _HasError;

        public bool HasError
        {
            get { return _HasError; }
            set
            {
                System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
                {

                    System.Threading.Thread.Sleep(500);
                    ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                    {
                        try
                        {
                            Updated?.Invoke(this);
                        }
                        catch (Exception ex)
                        {
                            AutoLogger.LogError(ex, "ValidationCheckRule HasError");
                        }
                    });

                });
                task.Start();
                _HasError = value;
            }
        }
        public bool IsWarning { get; set; }
        public int ValidationGroup { get; set; }
        public string CustomContent { get; set; }
        public object CustomValue { get; set; }
        public bool CanManualValidate { get; set; }

        public Action<IValidationRule> Updated { get; set; }
        //obj1 = datacontext, obj2 = new value
        public Func<object, object, string> GetCustomValidationMessageFunction { get; set; }

        public string ChangedPropertyName { get; set; }

        public object DataContext { get; set; }
    }
}
