using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using PiPlanningApp.Models;

namespace PiPlanningApp.Converters;

internal class FeaturesIterationsUserStoriesToGridElementMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is not null)
        {
            var compositeCollection = new CompositeCollection();
            values.OfType<IList>().ToList().ForEach(x => compositeCollection.Add(new CollectionContainer { Collection = x }));
            return compositeCollection;
        }
        return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}