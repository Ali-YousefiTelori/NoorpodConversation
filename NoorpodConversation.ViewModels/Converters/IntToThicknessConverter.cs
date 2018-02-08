using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace NoorpodConversation.ViewModels.Converters
{
    /// <summary>
    /// محل تغییر
    /// </summary>
    public enum IntToThicknessModeEnum
    {
        Left,
        Right,
        Top,
        Buttom,
        LeftRight,
        TopButtom,
        All
    }
    /// <summary>
    /// تبدیل یک داده ی عددی به Thichness
    /// </summary>
    public class IntToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var val = (double)value;
            var paramVal = parameter == null ? 0 : (double)parameter;
            Thickness thickness = new Thickness();
            if (Mode == IntToThicknessModeEnum.All)
                return new Thickness(val);
            else if (Mode == IntToThicknessModeEnum.Buttom)
            {
                thickness = new Thickness(0, 0, 0, val);
            }
            if (Mode == IntToThicknessModeEnum.Left)
                thickness = new Thickness(val, Top, 0, 0);
            if (Mode == IntToThicknessModeEnum.LeftRight)
                thickness = new Thickness(val, 0, val, 0);
            if (Mode == IntToThicknessModeEnum.Right)
                thickness = new Thickness(0, 0, val, 0);
            if (Mode == IntToThicknessModeEnum.Top)
                thickness = new Thickness(0, val, 0, 0);
            if (Mode == IntToThicknessModeEnum.TopButtom)
                thickness = new Thickness(0, val, 0, val);

            if (parameter != null)
            {
                if (ParameterMode == IntToThicknessModeEnum.Buttom)
                {
                    thickness.Bottom = paramVal;
                }
                if (ParameterMode == IntToThicknessModeEnum.Left)
                    thickness.Left = paramVal;
                if (ParameterMode == IntToThicknessModeEnum.LeftRight)
                {
                    thickness.Left = paramVal;
                    thickness.Right = paramVal;
                }
                if (ParameterMode == IntToThicknessModeEnum.Right)
                    thickness.Right = paramVal;
                if (ParameterMode == IntToThicknessModeEnum.Top)
                    thickness.Top = paramVal;

                if (ParameterMode == IntToThicknessModeEnum.TopButtom)
                {
                    thickness.Bottom = paramVal;
                    thickness.Top = paramVal;
                }
            }

            return thickness;
        }
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
        public IntToThicknessModeEnum Mode { get; set; }

        public double Top { get; set; }
        public IntToThicknessModeEnum ParameterMode { get; set; }
    }
}