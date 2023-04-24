using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

using PiPlanningApp.Commands;
using PiPlanningApp.Models;
using PiPlanningApp.Repositories;
using PiPlanningApp.Types;

namespace PiPlanningApp;

internal class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IInformationRepository informationRepository;

    public ObservableCollection<Iteration> Iterations { get; set; } = new();
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

    public MainWindowViewModel() : this(new JsonInformationRepository(ConfigurationManager.AppSettings["InformationFile"]))
    {
    }

    internal MainWindowViewModel(IInformationRepository informationRepository)
    {
        this.informationRepository = informationRepository;

        this.informationRepository.ReadInformation();
        this.informationRepository.ApplicationInformation.Features.ForEach(this.AddNewFeature);
        this.informationRepository.ApplicationInformation.Iterations.ForEach(this.AddNewIteration);
        this.IterationFeatureSlots.Clear();
        this.informationRepository.ApplicationInformation.IterationFeatureSlots.ForEach(this.IterationFeatureSlots.Add);

        this.AddFeatureCommand = new RelayCommand(_ => this.AddAndSaveNewFeature());
        this.AddIterationCommand = new RelayCommand(_ => this.AddAndSaveNewIteration());
        this.AddUserStoryCommand = new RelayCommand(this.AddAndSaveNewUserStory);
        this.DeleteFeatureCommand = new RelayCommand(this.RemoveAndSaveFeature, _ => this.Features.Count > 1);
        this.DeleteIterationCommand = new RelayCommand(this.RemoveAndSaveIteration, _ => this.Iterations.Count > 1);
        this.DeleteUserStoryCommand = new RelayCommand(this.RemoveAndSaveUserStory);
        this.EditFeatureCommand = new RelayCommand(this.EditAndSaveFeature);
        this.EditIterationCommand = new RelayCommand(this.EditAndSaveIteration);
        this.EditUserStoryCommand = new RelayCommand(this.EditAndSaveUserStory);
    }

    private void EditAndSaveUserStory(object obj)
    {
        if (obj is not UserStory userStoryToEdit)
        {
            return;
        }
        this.ClearOtherItemsInEdition(obj);

        userStoryToEdit.IsEditing = !userStoryToEdit.IsEditing;
        if (!userStoryToEdit.IsEditing)
        {
            var featureIterationSlot = this.IterationFeatureSlots.First(x => x.UserStories.Contains(userStoryToEdit));
            var parentIteration = this.Iterations.First(x => x.Id == featureIterationSlot.ParentIterationId);
            var parentFeature = this.Features.First(x => x.Id == featureIterationSlot.ParentFeatureId);

            parentIteration.LoadCapacity = this.IterationFeatureSlots.Where(x => x.ParentIterationId == parentIteration.Id).Sum(x => x.UserStories.Sum(y => y.StoryPoints));
            parentFeature.TotalStoryPoints = this.IterationFeatureSlots.Where(x => x.ParentFeatureId == parentFeature.Id).Sum(x => x.UserStories.Sum(y => y.StoryPoints));

            this.informationRepository.SaveChanges();
        }
    }

    private void EditAndSaveIteration(object obj)
    {
        if (obj is not Iteration iterationToEdit)
        {
            return;
        }
        this.ClearOtherItemsInEdition(obj);

        iterationToEdit.IsEditing = !iterationToEdit.IsEditing;
        if (!iterationToEdit.IsEditing)
        {
            this.informationRepository.SaveChanges();
        }
    }

    private void EditAndSaveFeature(object obj)
    {
        if (obj is not Feature featureToEdit)
        {
            return;
        }
        this.ClearOtherItemsInEdition(obj);

        featureToEdit.IsEditing = !featureToEdit.IsEditing;
        if (!featureToEdit.IsEditing)
        {
            this.informationRepository.SaveChanges();
        }
    }

    private void RemoveAndSaveIteration(object obj)
    {
        if (obj is not Iteration iteration)
        {
            return;
        }
        this.Iterations.Remove(iteration);
        foreach (var iterationToUpdate in this.Iterations)
        {
            iterationToUpdate.ColumnPosition = this.Iterations.IndexOf(iterationToUpdate);
            iterationToUpdate.IterationNumber = this.Iterations.IndexOf(iterationToUpdate) + 1;
        }
        foreach (var iterationFeatureSlot in this.IterationFeatureSlots.ToList())
        {
            if (iterationFeatureSlot.ParentIterationId == iteration.Id)
            {
                this.IterationFeatureSlots.Remove(iterationFeatureSlot);
                this.informationRepository.ApplicationInformation.IterationFeatureSlots.Remove(iterationFeatureSlot);
                continue;
            }
            iterationFeatureSlot.ColumnPosition = this.Iterations.Single(iteration => iteration.Id == iterationFeatureSlot.ParentIterationId).ColumnPosition;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Iterations.Count)));

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots.Count)));

        this.informationRepository.ApplicationInformation.Iterations.Remove(iteration);
        this.informationRepository.SaveChanges();
    }

    private void RemoveAndSaveFeature(object obj)
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
        foreach (var iterationFeatureSlot in this.IterationFeatureSlots.ToList())
        {
            if (iterationFeatureSlot.ParentFeatureId == feature.Id)
            {
                this.IterationFeatureSlots.Remove(iterationFeatureSlot);
                this.informationRepository.ApplicationInformation.IterationFeatureSlots.Remove(iterationFeatureSlot);
                continue;
            }
            iterationFeatureSlot.RowPosition = this.Features.Single(feature => feature.Id == iterationFeatureSlot.ParentFeatureId).RowPosition;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Features.Count)));

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IterationFeatureSlots.Count)));

        this.informationRepository.ApplicationInformation.Features.Remove(feature);
        this.informationRepository.SaveChanges();
    }

    private void RemoveAndSaveUserStory(object obj)
    {
        if (obj is not UserStory userStory)
        {
            return;
        }

        var iterationFeatureSlot = this.IterationFeatureSlots.First(iteration => iteration.UserStories.Contains(userStory));
        iterationFeatureSlot.RemoveUserStory(userStory);
        this.informationRepository.SaveChanges();
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

    private void AddAndSaveNewFeature()
    {
        var featureToAdd = new Feature
        {
            Id = Guid.NewGuid(),
            RowPosition = this.Features.Count,
            Title = "Feature Title",
            FeatureType = FeatureTypes.Feature
        };

        this.AddNewFeature(featureToAdd);
        this.informationRepository.ApplicationInformation.Features.Add(featureToAdd);
        this.informationRepository.ApplicationInformation.IterationFeatureSlots = this.IterationFeatureSlots.ToList();

        this.informationRepository.SaveChanges();
    }

    private void AddAndSaveNewIteration()
    {
        var iterationToAdd = new Iteration
        {
            Id = Guid.NewGuid(),
            ColumnPosition = this.Iterations.Count,
            IterationNumber = this.Iterations.Count + 1
        };

        this.AddNewIteration(iterationToAdd);
        this.informationRepository.ApplicationInformation.Iterations.Add(iterationToAdd);
        this.informationRepository.ApplicationInformation.IterationFeatureSlots = this.IterationFeatureSlots.ToList();

        this.informationRepository.SaveChanges();
    }

    private void AddAndSaveNewUserStory(object obj)
    {
        if (obj is not IterationFeatureSlot iterationFeatureSlot)
        {
            return;
        }

        var userStoryToAdd = new UserStory
        {
            Title = "User story"
        };

        iterationFeatureSlot.AddNewUserStory(userStoryToAdd);

        this.informationRepository.SaveChanges();
    }

    private void AddNewFeature(Feature featureToAdd)
    {
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

    private void AddNewIteration(Iteration iterationToAdd)
    {
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

    public void SaveChanges()
    {
        this.informationRepository.SaveChanges();
    }
}
