using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace PiPlanningApp.Converters
{
    internal class GridColumnOrRowsToStartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int integerValue) 
            {
                return string.Join(",", Enumerable.Range(0, integerValue));
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
