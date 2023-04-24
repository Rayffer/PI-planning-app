using System;
using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Newtonsoft.Json;

using PiPlanningApp.Types;

namespace PiPlanningApp.Models;

public partial class Feature : ObservableObject
{
    [ObservableProperty]
    private int rowPosition;

    [ObservableProperty]
    private Guid id;

    [ObservableProperty]
    private string title;

    private bool isEditing;

    [ObservableProperty]
    private FeatureTypes featureType;

    [ObservableProperty]
    private decimal totalStoryPoints;

    [ObservableProperty]
    private int featureNumber;

    public int ColumnPosition => -1;

    [JsonIgnore]
    public bool IsEditing
    {
        get => this.isEditing;
        set
        {
            this.isEditing = value;
            this.OnPropertyChanged(nameof(this.IsEditing));
        }
    }
}