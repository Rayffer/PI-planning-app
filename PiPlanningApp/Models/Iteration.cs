using System;
using System.ComponentModel;

namespace PiPlanningApp.Models;

internal class Iteration : INotifyPropertyChanged
{
    private Guid id;
    private int columnPosition;
    private string iterationName;
    private bool isEditing;
    private double totalCapacity;
    private double loadCapacity;
    private int supportDays;
    private int defectDays;
    private int unplannedDays;

    public Guid Id
    {
        get => this.id;
        set
        {
            this.id = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Id)));
        }
    }

    public int ColumnPosition
    {
        get => this.columnPosition;
        set
        {
            this.columnPosition = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ColumnPosition)));
        }
    }

    public string IterationName
    {
        get => this.iterationName;
        set
        {
            this.iterationName = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationName)));
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

    public double TotalCapacity
    {
        get => this.totalCapacity;
        set
        {
            this.totalCapacity = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TotalCapacity)));
        }
    }

    public double LoadCapacity
    {
        get => this.loadCapacity;
        set
        {
            this.loadCapacity = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.LoadCapacity)));
        }
    }

    public int SupportDays
    {
        get => this.supportDays;
        set
        {
            this.supportDays = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SupportDays)));
        }
    }

    public int DefectDays
    {
        get => this.defectDays;
        set
        {
            this.defectDays = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DefectDays)));
        }
    }
    
    public int UnplannedDays
    {
        get => this.unplannedDays;
        set
        {
            this.unplannedDays = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UnplannedDays)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
