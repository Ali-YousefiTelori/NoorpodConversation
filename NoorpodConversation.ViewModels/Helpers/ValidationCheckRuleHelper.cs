using NoorpodConversation.ViewModels.ControlHelpers;
using NoorpodConversation.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace NoorpodConversation.ViewModels.Helpers
{
    public static class ValidationCheckRuleHelper
    {
        public static readonly DependencyProperty PlacementModeProperty = DependencyProperty.RegisterAttached(
            "PlacementMode",
            typeof(PlacementMode),
            typeof(ValidationCheckRuleHelper),
            new FrameworkPropertyMetadata(PlacementMode.Left));

        public static PlacementMode GetPlacementMode(FrameworkElement frameworkElement)
        {
            return (PlacementMode)frameworkElement.GetValue(PlacementModeProperty);
        }

        public static void SetPlacementMode(FrameworkElement frameworkElement, PlacementMode placementMode)
        {
            frameworkElement.SetValue(PlacementModeProperty, placementMode);
        }

        public static readonly DependencyProperty FlowDirectionProperty = DependencyProperty.RegisterAttached(
            "FlowDirection",
            typeof(FlowDirection),
            typeof(ValidationCheckRuleHelper),
            new FrameworkPropertyMetadata());

        public static FlowDirection GetFlowDirection(FrameworkElement frameworkElement)
        {
            return (FlowDirection)frameworkElement.GetValue(FlowDirectionProperty);
        }

        public static void SetFlowDirection(FrameworkElement frameworkElement, FlowDirection flowDirection)
        {
            frameworkElement.SetValue(FlowDirectionProperty, flowDirection);
        }

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached(
            "HorizontalOffset",
            typeof(double),
            typeof(ValidationCheckRuleHelper),
            new FrameworkPropertyMetadata(0.0));

        public static double GetHorizontalOffset(FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(FrameworkElement frameworkElement, double value)
        {
            frameworkElement.SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached(
            "VerticalOffset",
            typeof(double),
            typeof(ValidationCheckRuleHelper),
            new FrameworkPropertyMetadata(0.0));

        public static double GetVerticalOffset(FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.GetValue(VerticalOffsetProperty);
        }

        public static void SetVerticalOffset(FrameworkElement frameworkElement, double value)
        {
            frameworkElement.SetValue(VerticalOffsetProperty, value);
        }




        public static ANotifyPropertyChanged GetViewModelBinding(ValidationCheckRule validationCheckRule)
        {
            return null;
        }

        public static ANotifyPropertyChanged GetViewModelObjectBinding(ValidationCheckRule validationCheckRule)
        {
            return null;
        }

        static Dictionary<ValidationCheckRule, ANotifyPropertyChanged> changesOfValidations = new Dictionary<ValidationCheckRule, ANotifyPropertyChanged>();
        static ValidationCheckRule FindByPropertyChanged(ANotifyPropertyChanged pChanged)
        {
            foreach (var item in changesOfValidations)
            {
                if (item.Value == pChanged)
                    return item.Key;
            }
            return null;
        }
        public static void SetViewModelBinding(ValidationCheckRule validationCheckRule, ANotifyPropertyChanged pChanged)
        {
            if (!changesOfValidations.ContainsKey(validationCheckRule))
            {
                changesOfValidations.Add(validationCheckRule, pChanged);
            }
            pChanged.AddValidation(validationCheckRule, pChanged);
            pChanged.RemovedValidationAction = (p) =>
            {
                changesOfValidations.Remove(validationCheckRule);
            };
        }

        public static void SetViewModelObjectBinding(ValidationCheckRule validationCheckRule, CustomViewModelHelepr customViewModelHelepr)
        {
            if (customViewModelHelepr == null)
                return;
            if (customViewModelHelepr.ControlData == null)
            {
                customViewModelHelepr.ControlDataChanged += (ctrl) =>
                {
                    if (customViewModelHelepr.ControlData.DataContext == null)
                    {
                        customViewModelHelepr.ControlData.DataContextChanged += (s, e) =>
                        {
                            if (customViewModelHelepr.ControlData.DataContext == null)
                                return;
                            SetViewModelBinding(validationCheckRule, customViewModelHelepr.ControlData.DataContext as ANotifyPropertyChanged);
                        };
                    }
                    else
                    {
                        SetViewModelBinding(validationCheckRule, customViewModelHelepr.ControlData.DataContext as ANotifyPropertyChanged);
                    }
                };
            }
            else
            {
                if (customViewModelHelepr.ControlData.DataContext is ANotifyPropertyChanged)
                    SetViewModelBinding(validationCheckRule, customViewModelHelepr.ControlData.DataContext as ANotifyPropertyChanged);
            }
        }

        public static int GetValidationGroup(ValidationCheckRule validationCheckRule)
        {
            return -1;
        }

        public static void SetValidationGroup(ValidationCheckRule validationCheckRule, int validationGroup)
        {
            validationCheckRule.ValidationGroup = validationGroup;
        }

        public static object GetPathName(ValidationCheckRule validationCheckRule)
        {
            return null;
        }

        public static void SetPathName(ValidationCheckRule validationCheckRule, string name)
        {
            if (changesOfValidations.ContainsKey(validationCheckRule))
            {
                validationCheckRule.ChangedPropertyName = name;
                //var gp = changesOfValidations[validationCheckRule];
                //var pv = ViewsUtility.GetPropertyValue<ANotifyPropertyChanged>(gp, name);
                //validationCheckRule.ChangedProperty = (ANotifyPropertyChanged)pv;
            }
        }
    }
}
