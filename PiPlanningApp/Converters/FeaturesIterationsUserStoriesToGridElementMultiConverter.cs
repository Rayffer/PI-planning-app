using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

using PiPlanningApp.Models;

namespace PiPlanningApp.Converters
{
    internal class FeaturesIterationsUserStoriesToGridElementMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] is ObservableCollection<Feature> features &&
                values[1] is ObservableCollection<Iteration> iterations && 
                values[2] is ObservableCollection<IterationFeatureSlot> iterationFeatureSlots)
            { 
                var compositeCollection = new CompositeCollection
                {
                    new CollectionContainer { Collection = features },
                    new CollectionContainer { Collection = iterations },
                    new CollectionContainer { Collection = iterationFeatureSlots},
                };
                return compositeCollection;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
