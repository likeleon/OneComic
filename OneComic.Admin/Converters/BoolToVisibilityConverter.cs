using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OneComic.Admin.Converters
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolean = false;
            if (value is bool)
                boolean = (bool)value;
            else if (value is bool?)
                boolean = ((bool?)value).GetValueOrDefault(false);
            return boolean ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility)value == Visibility.Visible;
            else
                return false;
        }
    }
}
