using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using PiPlanningApp.Commands;
using PiPlanningApp.Models;
using PiPlanningApp.Types;

namespace PiPlanningApp;

internal class MainWindowViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Iteration> Iterations { get; set; } = new() {
        new Iteration
        {
            ColumnPosition = 0,
            Id = Guid.NewGuid()
        },
        new Iteration
        {
            ColumnPosition = 1,
            Id = Guid.NewGuid()
        },
        new Iteration
        {
            ColumnPosition = 2,
            Id = Guid.NewGuid()
        },
        new Iteration
        {
            ColumnPosition = 3,
            Id = Guid.NewGuid()
        },
        new Iteration
        {
            ColumnPosition = 4,
            Id = Guid.NewGuid()
        },
        new Iteration
        {
            ColumnPosition = 5,
            Id = Guid.NewGuid(),
            IterationName = "IP"
        },
    };
    public ObservableCollection<Feature> Features { get; set; } = new();
    public ObservableCollection<IterationFeatureSlot> IterationFeatureSlots { get; set; } = new();

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
        this.AddNewFeature();

        this.AddFeatureCommand = new RelayCommand(_ => this.AddNewFeature());
        this.AddIterationCommand = new RelayCommand(_ => this.AddNewIteration());
        this.AddUserStoryCommand = new RelayCommand(this.AddNewUserStory);
        this.DeleteFeatureCommand = new RelayCommand(this.RemoveFeature, _ => this.Features.Count > 1);
        this.DeleteIterationCommand = new RelayCommand(this.RemoveIteration, _ => this.Iterations.Count > 1);
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
        foreach (var iterationFeatureSlot in this.IterationFeatureSlots.ToList())
        {
            if (iterationFeatureSlot.ParentIterationId == iteration.Id)
            {
                this.IterationFeatureSlots.Remove(iterationFeatureSlot);
                continue;
            }
            iterationFeatureSlot.ColumnPosition = this.Iterations.Single(iteration => iteration.Id == iterationFeatureSlot.ParentIterationId).ColumnPosition;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations.Count)));

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots.Count)));
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
            featureToUpdate.RowPosition = this.Features.IndexOf(featureToUpdate);
        }
        foreach (var iterationFeatureSlot in this.IterationFeatureSlots)
        {
            if (iterationFeatureSlot.ParentFeatureId == feature.Id)
            {
                this.IterationFeatureSlots.Remove(iterationFeatureSlot);
                continue;
            }
            iterationFeatureSlot.RowPosition = this.Features.Single(feature => feature.Id == iterationFeatureSlot.ParentFeatureId).RowPosition;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features.Count)));

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots.Count)));
    }

    private void RemoveUserStory(object obj)
    {
        if (obj is not UserStory userStory)
        {
            return;
        }

        var iterationFeatureSlot = this.IterationFeatureSlots.First(iteration => iteration.UserStories.Contains(userStory));
        iterationFeatureSlot.RemoveUserStory(userStory);
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
        foreach (var iterationFeatureSlot in this.IterationFeatureSlots)
            foreach (var userStory in iterationFeatureSlot.UserStories)
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
        var featureToAdd = new Feature
        {
            Id = Guid.NewGuid(),
            RowPosition = this.Features.Count,
            Title = "Feature Title",
            FeatureType = FeatureTypes.Feature
        };
        this.Features.Add(featureToAdd);

        foreach (var iteration in this.Iterations)
        {
            this.IterationFeatureSlots.Add(new IterationFeatureSlot
            {
                ColumnPosition = iteration.ColumnPosition,
                ParentIterationId = iteration.Id,
                ParentFeatureId = featureToAdd.Id,
                RowPosition = featureToAdd.RowPosition,
                UserStories = new()
            });
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features.Count)));

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots.Count)));
    }

    private void AddNewIteration()
    {
        var iterationToAdd = new Iteration
        {
            Id = Guid.NewGuid(),
            ColumnPosition = this.Iterations.Count
        };
        this.Iterations.Add(iterationToAdd);

        foreach (var feature in this.Features)
        {
            this.IterationFeatureSlots.Add(new IterationFeatureSlot
            {
                ColumnPosition = iterationToAdd.ColumnPosition,
                ParentIterationId = iterationToAdd.Id,
                ParentFeatureId = feature.Id,
                RowPosition = feature.RowPosition,
                UserStories = new()
            });
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations.Count)));

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots.Count)));
    }

    private void AddNewUserStory(object obj)
    {
        if (obj is not IterationFeatureSlot iterationFeatureSlot)
        {
            return;
        }

        iterationFeatureSlot.AddNewUserStory(new UserStory { Title = $"User story {iterationFeatureSlot.RowPosition}.{iterationFeatureSlot.ColumnPosition}.{iterationFeatureSlot.UserStories.Count}" });
    }
}
