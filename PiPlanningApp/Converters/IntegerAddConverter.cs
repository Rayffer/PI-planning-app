using System;
using System.Globalization;
using System.Windows.Data;

namespace PiPlanningApp.Converters;

internal class IntegerAddConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int integerValue)
        {
            return 1 + integerValue;
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
