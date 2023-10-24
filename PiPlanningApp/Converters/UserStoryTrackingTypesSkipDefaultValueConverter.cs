namespace PiPlanningApp.Converters;

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using PiPlanningApp.Types;

internal class UserStoryTrackingTypesSkipDefaultValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is UserStoryTrackingTypes[] featureTypes)
        {
            return featureTypes.Where(x => x != default).ToArray();
        }
        return null;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
