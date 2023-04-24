using System;
using System.ComponentModel;

using Newtonsoft.Json;

using PiPlanningApp.Types;

namespace PiPlanningApp.Models;

public class Feature : INotifyPropertyChanged
{
    private int rowPosition;
    private Guid id;
    private string title;
    private bool isEditing;
    private FeatureTypes featureType;
    private decimal totalStoryPoints;
    private int featureNumber;

    public Guid Id
    {
        get => this.id;
        set
        {
            this.id = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Id)));
        }
    }

    public int ColumnPosition => -1;

    public int RowPosition
    {
        get => this.rowPosition;
        set
        {
            this.rowPosition = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RowPosition)));
        }
    }

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

    public FeatureTypes FeatureType
    {
        get => this.featureType;
        set
        {
            this.featureType = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FeatureType)));
        }
    }

    public decimal TotalStoryPoints
    {
        get => this.totalStoryPoints;
        set
        {
            this.totalStoryPoints = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TotalStoryPoints)));
        }
    }

    public int FeatureNumber
    {
        get => this.featureNumber;
        set
        {
            this.featureNumber = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FeatureNumber)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}