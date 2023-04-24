using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using PiPlanningApp.Models;

namespace PiPlanningApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void Grid_MouseMove(object sender, MouseEventArgs e)
    {
        if (sender is Grid gridElement &&
            gridElement.DataContext is UserStory userStoryToMove &&
            this.DataContext is MainWindowViewModel mainWindowViewModel &&
            !userStoryToMove.IsEditing &&
            e.LeftButton == MouseButtonState.Pressed)
        {
            var slotToDragFrom = mainWindowViewModel.IterationFeatureSlots.First(slot => slot.UserStories.Contains(userStoryToMove));

            DragDrop.DoDragDrop(gridElement, userStoryToMove, DragDropEffects.Move);
            slotToDragFrom.RemoveUserStory(userStoryToMove);

            var previousParentIteration = mainWindowViewModel.Iterations.First(x => x.Id == slotToDragFrom.ParentIterationId);
            var previousParentFeature = mainWindowViewModel.Features.First(x => x.Id == slotToDragFrom.ParentFeatureId);

            previousParentIteration.LoadCapacity = mainWindowViewModel.IterationFeatureSlots.Where(x => x.ParentIterationId == previousParentIteration.Id)
                                                                                            .Sum(x => x.UserStories.Sum(y => y.StoryPoints));
            previousParentFeature.TotalStoryPoints = mainWindowViewModel.IterationFeatureSlots.Where(x => x.ParentFeatureId == previousParentFeature.Id)
                                                                                              .Sum(x => x.UserStories.Sum(y => y.StoryPoints));

            mainWindowViewModel.SaveChanges();
        }
    }

    private void Grid_Drop(object sender, DragEventArgs e)
    {
        if (sender is Grid gridElement &&
            gridElement.DataContext is IterationFeatureSlot slotToDropInto &&
            this.DataContext is MainWindowViewModel mainWindowViewModel &&
            e.Data is not null)
        {
            var userStoryToDrop = e.Data.GetData(typeof(UserStory)) as UserStory;
            slotToDropInto.AddNewUserStory(userStoryToDrop);

            var newParentIteration = mainWindowViewModel.Iterations.First(x => x.Id == slotToDropInto.ParentIterationId);
            var newParentFeature = mainWindowViewModel.Features.First(x => x.Id == slotToDropInto.ParentFeatureId);

            newParentIteration.LoadCapacity =
                mainWindowViewModel.IterationFeatureSlots.Where(x => x.ParentIterationId == newParentIteration.Id)
                                                         .Sum(x => x.UserStories.Sum(y => y.StoryPoints));
            newParentFeature.TotalStoryPoints =
                mainWindowViewModel.IterationFeatureSlots.Where(x => x.ParentFeatureId == newParentFeature.Id)
                                                         .Sum(x => x.UserStories.Sum(y => y.StoryPoints));

            e.Handled = true;
        }
    }
}