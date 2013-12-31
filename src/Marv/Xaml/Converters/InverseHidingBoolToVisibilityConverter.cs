using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace Marv.Xaml.Converters
{
    public class InverseHidingBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = (bool)value;

            return isVisible ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}