using System.Text.Json.Serialization;

using CommunityToolkit.Mvvm.ComponentModel;

using PiPlanningApp.Types;

namespace PiPlanningApp.Models;

public partial class UserStory : ObservableObject
{
    [ObservableProperty]
    private string title;

    private bool isEditing;

    [ObservableProperty]
    private decimal storyPoints;

    [ObservableProperty]
    private decimal days;

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

    [ObservableProperty]
    private UserStoryTrackingTypes userStoryTrackingType;
}
