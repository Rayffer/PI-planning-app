using System;
using System.Globalization;
using System.Windows.Data;

using PiPlanningApp.Models;

namespace PiPlanningApp.Converters;

internal class IterationTitleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Iteration iteration)
        {
            return $"Iteration {(string.IsNullOrEmpty(iteration.IterationName) ? iteration.ColumnPosition + 1 : iteration.IterationName)}";
        }
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
