using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PiPlanningApp.Converters;

internal class IntegerGreaterThanOrEqualToOtherVisibilityMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 &&
            values[0] is decimal referenceValue &&
            values[1] is decimal comparisonValue)
        {
            return referenceValue.CompareTo(comparisonValue) == -1 ? Visibility.Visible : Visibility.Collapsed;
        }
        return 0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}