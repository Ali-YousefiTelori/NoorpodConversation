using NoorpodConversation.BaseViewModels.Collections;
using NoorpodConversation.BaseViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// پروپرتی چنج برای بایندیگ های یک کلاس به کنترل های WPF
    /// </summary>
    public abstract class ANotifyPropertyChanged : INotifyPropertyChanged
    {
        public static event Action<object> Removed;
        public Func<object, object, string> GetConfilictErrorMessageFunction { get; set; }

        static ANotifyPropertyChanged()
        {
            FastCollectionRemoved.RemovedForValidationAction = (obj) =>
            {
                if (obj is ANotifyPropertyChanged)
                {
                    ((ANotifyPropertyChanged)obj).RemoveFromValidationItem();
                }
            };
        }

        public static Action<object, object, object, object, object> DispatcherBeginInvokeAction { get; set; }
        public Action<ANotifyPropertyChanged> RemovedValidationAction { get; set; }
        public ANotifyPropertyChanged()
        {
            Validations = new List<IValidationRule>();
        }

        public bool IsStoryBoardComeplete { get; set; }
        public Action StoryBoardComepleteAction { get; set; }
        static bool isStoped = false;

        static Dictionary<ANotifyPropertyChanged, List<string>> items = new Dictionary<ANotifyPropertyChanged, List<string>>();

        public static void StopNotifyChanging()
        {
            isStoped = true;
        }

        public static void StartNotifyChanging()
        {
            isStoped = false;
            foreach (var notify in items)
            {
                foreach (var pName in notify.Value)
                {
                    notify.Key.OnPropertyChanged(pName);
                }
            }
            items.Clear();
        }


        bool _canClick = true;

        public bool CanClick
        {
            get { return _canClick; }
            set { _canClick = value; OnPropertyChanged("CanClick"); }
        }

        public bool IgnoreStopChanged { get; set; }

        public void OnPropertyChanged(string propertyName)
        {
            if (isStoped && !IgnoreStopChanged)
            {
                if (items.ContainsKey(this))
                {
                    if (!items[this].Contains(propertyName))
                        items[this].Add(propertyName);
                }
                else
                    items.Add(this, new List<string>() { propertyName });
            }
            else
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        #region Validations Members


        public ANotifyPropertyChanged ValidationErrorsChanged
        {
            get
            {
                return this;
            }
        }

        public List<IValidationRule> Validations { get; set; }

        /// <summary>
        /// بررسی ولیدیشن ها 
        /// </summary>
        /// <param name="validationGroup">گروه ولیدیشن مورد نظر</param>
        /// <returns>در صورتی که خطا داشته باشد مقدار درست بر میگرداند</returns>
        public bool HasError(int validationGroup)
        {
            foreach (var item in Validations.Where(x => x.ValidationGroup == validationGroup && !x.IsWarning))
            {
                item.ManualValidate(null);
                if (item.HasError)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// single validation
        /// </summary>
        /// <param name=""></param>
        public void ClearExistClassName(IValidationRule validation)
        {
            var valids = from x in Validations where x.GetType().FullName == validation.GetType().FullName select x;
            Validations.RemoveAll(x => valids.Contains(x));
        }

        public void AddValidation(IValidationRule validation, ANotifyPropertyChanged dataContext)
        {
            ClearExistClassName(validation);
            validation.DataContext = dataContext;
            Validations.Add(validation);
            validation.GetCustomValidationMessageFunction = GetConfilictErrorMessageFunction;
            validation.Updated = (v) =>
            {
                if (v.ChangedPropertyName != null)
                {
                    //v.ChangedProperty.OnPropertyChanged("ValidationErrorsChanged");
                    var pv = GetPropertyValue<ANotifyPropertyChanged>(this, v.ChangedPropertyName);
                    pv.OnPropertyChanged("ValidationErrorsChanged");

                }
                else
                {
                    OnPropertyChanged("ValidationErrorsChanged");
                }
            };
        }

        public void RemoveValidation(IValidationRule validation)
        {
            Validations.Remove(validation);
            validation.Updated = null;
            validation.DataContext = null;
            validation.GetCustomValidationMessageFunction = null;
        }

        public void RemoveFromValidationItem()
        {
            List<IValidationRule> valids = (from x in Validations where x.DataContext == this select x).ToList();
            if (valids.Count == 0)
                return;
            Validations.RemoveAll(x => valids.Contains(x));
            foreach (var item in valids)
            {
                item.Updated = null;
            }
            RemovedValidationAction?.Invoke(this);
        }

        public static T GetPropertyValue<T>(object src, string propName)
        {
            return (T)src.GetType().GetProperty(propName).GetValue(src, null);
        }

        #endregion
    }
}
