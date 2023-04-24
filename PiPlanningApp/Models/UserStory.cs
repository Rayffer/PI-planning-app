using System.ComponentModel;

using Newtonsoft.Json;

namespace PiPlanningApp.Models;

public class UserStory : INotifyPropertyChanged
{
    private string title;
    private bool isEditing;
    private decimal storyPoints;

    public string Title
    {
        get => this.title;
        set
        {
            this.title = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Title)));
        }
    }

    [JsonIgnore]
    public bool IsEditing
    {
        get => this.isEditing;
        set
        {
            this.isEditing = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsEditing)));
        }
    }

    public decimal StoryPoints
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