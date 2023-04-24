using System.Collections.Generic;

namespace PiPlanningApp.Models;

public class ApplicationInformation
{
    public List<Feature> Features { get; set; } = new List<Feature>();
    public List<Iteration> Iterations { get; set; } = new List<Iteration>();
    public List<IterationFeatureSlot> IterationFeatureSlots { get; set; } = new List<IterationFeatureSlot>();
}