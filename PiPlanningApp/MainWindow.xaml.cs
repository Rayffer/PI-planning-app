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

            var result = DragDrop.DoDragDrop(gridElement, userStoryToMove, DragDropEffects.Move);
            if (result == DragDropEffects.None)
            {
                e.Handled = true;
                return;
            }
            slotToDragFrom.RemoveUserStory(userStoryToMove);

            var previousParentIteration =
                mainWindowViewModel.Iterations.First(x => x.Id == slotToDragFrom.ParentIterationId);
            var previousParentFeature =
                mainWindowViewModel.Features.First(x => x.Id == slotToDragFrom.ParentFeatureId);

            previousParentIteration.LoadCapacity =
                mainWindowViewModel.IterationFeatureSlots.Where(x => x.ParentIterationId == previousParentIteration.Id)
                                                         .Sum(x => x.UserStories.Sum(y => y.StoryPoints));
            previousParentFeature.TotalStoryPoints =
                mainWindowViewModel.IterationFeatureSlots.Where(x => x.ParentFeatureId == previousParentFeature.Id)
                                                         .Sum(x => x.UserStories.Sum(y => y.StoryPoints));

            mainWindowViewModel.SaveChanges();

            e.Handled = true;
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

    private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel mainWindowViewModel)
        {
            e.Handled = true;
            return;
        }
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            if (this.MainGridScaleTransform.ScaleX >= 0.1 &&
                this.MainGridScaleTransform.ScaleX <= 4 &&
                this.MainGridScaleTransform.ScaleY >= 0.1 &&
                this.MainGridScaleTransform.ScaleY <= 4)
            {
                var previousScaleX = this.MainGridScaleTransform.ScaleX;
                var previousScaleY = this.MainGridScaleTransform.ScaleX;

                var modifiedScaleX = previousScaleX + e.Delta / 1200D;
                var modifiedScaleY = previousScaleY + e.Delta / 1200D;
                modifiedScaleX = modifiedScaleX switch
                {
                    < 0.3 => 0.3,
                    > 4.0 => 4.0,
                    _ => modifiedScaleX
                };
                modifiedScaleY = modifiedScaleY switch
                {
                    < 0.3 => 0.3,
                    > 4.0 => 4.0,
                    _ => modifiedScaleY
                };

                if (previousScaleX == modifiedScaleX && previousScaleY == modifiedScaleY)
                {
                    e.Handled = true;
                    return;
                }

                //var mouseMainGridPosition = e.GetPosition(this.MainGrid);
                //var mouseMainWindowPosition = e.GetPosition(sender as ScrollViewer);

                //this.MainGridScaleTransform.CenterX = mouseMainGridPosition.X - this.MainGrid.ActualWidth / 2;
                //this.MainGridScaleTransform.CenterY = mouseMainGridPosition.Y - this.MainGrid.ActualHeight / 2;

                //this.MainGridTranslateTransform.X = (this.MainGridTranslateTransform.X - this.MainGrid.ActualWidth / 2) * modifiedScaleX / previousScaleX;
                //this.MainGridTranslateTransform.Y = (this.MainGridTranslateTransform.Y - this.MainGrid.ActualHeight / 2) * modifiedScaleY / previousScaleY;
                mainWindowViewModel.SetScale(modifiedScaleX, modifiedScaleY);
            }
        }
        else
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                mainWindowViewModel.SetTranslateOffset(this.MainGridTranslateTransform.X + e.Delta, this.MainGridTranslateTransform.Y);
            }
            else
            {
                mainWindowViewModel.SetTranslateOffset(this.MainGridTranslateTransform.X, this.MainGridTranslateTransform.Y + e.Delta);
            }
        }
        e.Handled = true;
    }

    private Point previousPosition;

    private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel mainWindowViewModel)
        {
            e.Handled = true;
            return;
        }
        if (Mouse.MiddleButton != MouseButtonState.Pressed)
        {
            e.Handled = true;
            this.previousPosition = default;
            return;
        }
        if (this.previousPosition == default)
        {
            this.previousPosition = e.GetPosition(this);
            e.Handled = true;
            return;
        }

        var currentPosition = e.GetPosition(this);
        this.MainWindowViewModel.SetTranslateOffset(
            this.MainGridTranslateTransform.X + (currentPosition.X - this.previousPosition.X) / this.MainGridScaleTransform.ScaleX,
            this.MainGridTranslateTransform.Y + (currentPosition.Y - this.previousPosition.Y) / this.MainGridScaleTransform.ScaleY);

        this.previousPosition = currentPosition;
    }
}