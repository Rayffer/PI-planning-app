using System.Windows;
using System.Windows.Controls;

using PiPlanningApp.Models;

namespace PiPlanningApp.DataTemplateSelectors
{
    public class GridElementDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FeatureTemplate { get; set; }
        public DataTemplate IterationTemplate { get; set; }
        public DataTemplate IterationFeatureSlotTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                Feature => this.FeatureTemplate,
                Iteration => this.IterationTemplate,
                IterationFeatureSlot=> this.IterationFeatureSlotTemplate,
                _ => base.SelectTemplate(item, container)
            };
        }
    }
}
