using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using PiPlanningApp.Types;

namespace PiPlanningApp.Converters;

internal class FeatureTypesSkipDefaultValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is FeatureTypes[] featureTypes)
        {
            return featureTypes.Where(x => x != default).ToArray();
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}