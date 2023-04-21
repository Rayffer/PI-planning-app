using System;
using System.ComponentModel;

namespace PiPlanningApp.Models;

internal class UserStory : INotifyPropertyChanged
{
    private string title;
    private bool isEditing;
    private double storyPoints;

    public string Title
    {
        get => this.title;
        set
        {
            this.title = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Title)));
        }
    }

    public bool IsEditing
    {
        get => this.isEditing; set
        {
            this.isEditing = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsEditing)));
        }
    }

    public double StoryPoints
    {
        get => this.storyPoints;
        set
        {
            this.storyPoints = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.StoryPoints)));
        }
    }



    public event PropertyChangedEventHandler PropertyChanged;
}
