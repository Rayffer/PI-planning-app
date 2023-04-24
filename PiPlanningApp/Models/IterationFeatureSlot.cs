using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace PiPlanningApp.Models;

public partial class IterationFeatureSlot : ObservableObject
{
    [ObservableProperty]
    private Guid parentFeatureId;

    [ObservableProperty]
    private Guid parentIterationId;

    [ObservableProperty]
    private int rowPosition;

    [ObservableProperty]
    private int columnPosition;

    public ObservableCollection<UserStory> UserStories { get; set; } = new();

    public void RemoveUserStory(UserStory userStory)
    {
        this.UserStories.Remove(userStory);
        this.OnPropertyChanged(nameof(this.UserStories));
        this.OnPropertyChanged(nameof(this.UserStories.Count));
    }

    public void AddNewUserStory(UserStory userStory)
    {
        this.UserStories.Add(userStory);
        this.OnPropertyChanged(nameof(this.UserStories));
        this.OnPropertyChanged(nameof(this.UserStories.Count));
    }
}
