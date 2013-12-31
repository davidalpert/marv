using System.Linq;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Marv.Xaml.Converters
{
    public class HidingBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = (bool)value;

            return isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}