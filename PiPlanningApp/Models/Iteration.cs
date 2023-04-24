using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Newtonsoft.Json;

namespace PiPlanningApp.Models;

public partial class Iteration : ObservableObject
{
    [ObservableProperty]
    private Guid id;

    [ObservableProperty]
    private int columnPosition;

    [ObservableProperty]
    private string iterationName;

    private bool isEditing;

    [ObservableProperty]
    private decimal totalCapacity;

    [ObservableProperty]
    private decimal loadCapacity;

    [ObservableProperty]
    private int supportDays;

    [ObservableProperty]
    private int defectDays;

    [ObservableProperty]
    private int unplannedDays;

    [ObservableProperty]
    private int iterationNumber;

    public int RowPosition => -1;

    [JsonIgnore]
    public bool IsEditing
    {
        get => this.isEditing;
        set => this.SetProperty(ref this.isEditing, value);
    }
}
