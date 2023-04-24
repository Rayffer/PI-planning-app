using System;
using System.Globalization;
using System.Windows.Data;

using PiPlanningApp.Models;

namespace PiPlanningApp.Converters;

internal class IterationTitleMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2)
        {
            if (values[0] is null || string.IsNullOrEmpty(values[0].ToString()))
            {
                return $"Iteration {values[1]}";
            }
            else if (values[0] is not null)
            {
                return $"{values[0]} Iteration";
            }
        }
        return "";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}