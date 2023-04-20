using System;
using System.ComponentModel;

namespace PiPlanningApp.Models;

internal class UserStory : INotifyPropertyChanged
{
    private string title;
    private double storyPointsEstimation;
    private Guid parentFeatureId;
    private Guid parentIterationId;
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

    public double StoryPointsEstimation
    {
        get => this.storyPointsEstimation;
        set
        {
            this.storyPointsEstimation = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.StoryPointsEstimation)));
        }
    }

    public Guid ParentIterationId
    {
        get => this.parentIterationId;
        set
        {
            this.parentIterationId = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ParentIterationId)));
        }
    }

    public Guid ParentFeatureId
    {
        get => this.parentFeatureId;
        set
        {
            this.parentFeatureId = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ParentFeatureId)));
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
