using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PiPlanningApp.Converters;

internal class StringIsNotNullOrEmptyVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string stringtoConvert && !string.IsNullOrEmpty(stringtoConvert)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}