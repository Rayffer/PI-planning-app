using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using PiPlanningApp.Commands;
using PiPlanningApp.Models;
using PiPlanningApp.Types;

namespace PiPlanningApp;

internal class MainWindowViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Iteration> Iterations { get; set; } = new() {
        new Iteration { ColumnPosition = 0, Id = Guid.NewGuid() },
        new Iteration { ColumnPosition = 1, Id = Guid.NewGuid() },
        new Iteration { ColumnPosition = 2, Id = Guid.NewGuid() },
        new Iteration { ColumnPosition = 3, Id = Guid.NewGuid() },
        new Iteration { ColumnPosition = 4, Id = Guid.NewGuid() },
        new Iteration { ColumnPosition = 5, Id = Guid.NewGuid(), IterationName = "IP" },
    };
    public ObservableCollection<Feature> Features { get; set; } = new();
    public ObservableCollection<UserStory> UserStories { get; set; } = new();

    public RelayCommand AddFeatureCommand { get; }
    public RelayCommand AddIterationCommand { get; }
    public RelayCommand AddUserStoryCommand { get; }
    public RelayCommand DeleteFeatureCommand { get; }
    public RelayCommand DeleteIterationCommand { get; }
    public RelayCommand DeleteUserStoryCommand { get; }
    public RelayCommand EditFeatureCommand { get; }
    public RelayCommand EditIterationCommand { get; }
    public RelayCommand EditUserStoryCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public MainWindowViewModel()
    {
        this.AddFeatureCommand = new RelayCommand(_ => this.AddNewFeature());
        this.AddIterationCommand = new RelayCommand(_ => this.AddNewIteration());
        this.AddUserStoryCommand = new RelayCommand(_ => this.AddNewUserStory());
        this.DeleteFeatureCommand = new RelayCommand(this.RemoveFeature);
        this.DeleteIterationCommand = new RelayCommand(this.RemoveIteration);
        this.DeleteUserStoryCommand = new RelayCommand(this.RemoveUserStory);
        this.EditFeatureCommand = new RelayCommand(this.EditFeature);
        this.EditIterationCommand = new RelayCommand(this.EditIteration);
        this.EditUserStoryCommand = new RelayCommand(this.EditUserStory);
    }

    private void EditUserStory(object obj)
    {
        if (obj is not UserStory userStoryToEdit)
        {
            return;
        }
        this.ClearOtherItemsInEdition(obj);

        userStoryToEdit.IsEditing = !userStoryToEdit.IsEditing;
    }

    private void EditIteration(object obj)
    {
        if (obj is not Iteration iterationToEdit)
        {
            return;
        }
        this.ClearOtherItemsInEdition(obj);

        iterationToEdit.IsEditing = !iterationToEdit.IsEditing;
    }

    private void EditFeature(object obj)
    {
        if (obj is not Feature featureToEdit)
        {
            return;
        }
        this.ClearOtherItemsInEdition(obj);

        featureToEdit.IsEditing = !featureToEdit.IsEditing;
    }

    private void RemoveIteration(object obj)
    {
        if (obj is not Iteration iteration)
        {
            return;
        }
        this.Iterations.Remove(iteration);
        foreach (var iterationToUpdate in this.Iterations)
        {
            iterationToUpdate.ColumnPosition = this.Iterations.IndexOf(iterationToUpdate);
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations)));
    }

    private void RemoveFeature(object obj)
    {
        if (obj is not Feature feature)
        {
            return;
        }
        this.Features.Remove(feature);
        foreach (var featureToUpdate in this.Features)
        {
            var featureIndex = this.Features.IndexOf(featureToUpdate);
            featureToUpdate.RowPosition = featureIndex;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Feature.RowPosition)));
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features)));
    }

    private void RemoveUserStory(object obj)
    {
        if (obj is not Feature feature)
        {
            return;
        }
        this.Features.Remove(feature);
        foreach (var featureToUpdate in this.Features)
        {
            var featureIndex = this.Features.IndexOf(featureToUpdate);
            featureToUpdate.RowPosition = featureIndex;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Feature.RowPosition)));
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features)));
    }

    private void ClearOtherItemsInEdition(object obj)
    {
        foreach (var feature in this.Features)
        {
            if (feature == obj)
            {
                continue;
            }
            feature.IsEditing = false;
        }
        foreach (var iteration in this.Iterations)
        {
            if (iteration == obj)
            {
                continue;
            }
            iteration.IsEditing = false;
        }
        foreach (var userStory in this.UserStories)
        {
            if (userStory == obj)
            {
                continue;
            }
            userStory.IsEditing = false;
        }
    }

    private void AddNewFeature()
    {
        this.Features.Add(new Feature
        {
            Id = Guid.NewGuid(),
            RowPosition = this.Features.Count,
            Title = "Feature Title",
            FeatureType = FeatureTypes.Feature
        });
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features)));
    }

    private void AddNewIteration()
    {
        this.Iterations.Add(new Iteration { Id = Guid.NewGuid(), ColumnPosition = this.Iterations.Count });
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations)));
    }

    private void AddNewUserStory()
    {
        this.Iterations.Add(new Iteration { Id = Guid.NewGuid(), ColumnPosition = this.Iterations.Count });
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations)));
    }
}
