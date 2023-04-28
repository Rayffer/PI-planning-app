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
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            if (this.MainGridScaleTransform.ScaleX >= 0.1 &&
                this.MainGridScaleTransform.ScaleX <= 4 &&
                this.MainGridScaleTransform.ScaleY >= 0.1 &&
                this.MainGridScaleTransform.ScaleY <= 4)
            {
                this.MainGridScaleTransform.ScaleX += e.Delta / 1200D;
                this.MainGridScaleTransform.ScaleY += e.Delta / 1200D;

                this.MainGridScaleTransform.ScaleX = this.MainGridScaleTransform.ScaleX switch
                {
                    < 0.1 => 0.1,
                    > 4.0 => 4.0,
                    _ => this.MainGridScaleTransform.ScaleX
                };
                this.MainGridScaleTransform.ScaleY = this.MainGridScaleTransform.ScaleY switch
                {
                    < 0.1 => 0.1,
                    > 4.0 => 4.0,
                    _ => this.MainGridScaleTransform.ScaleY
                };
            }
        }
        else
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                this.MainGridTranslateTransform.X += e.Delta;

                if (this.MainGridTranslateTransform.X < (-(this.MainGrid.ActualWidth - this.ActualWidth) - this.margin))
                {
                    this.MainGridTranslateTransform.X = (-(this.MainGrid.ActualWidth - this.ActualWidth) - this.margin);
                }
                if (this.MainGridTranslateTransform.X > this.margin)
                {
                    this.MainGridTranslateTransform.X = this.margin;
                }
            }
            else
            {
                this.MainGridTranslateTransform.Y += e.Delta;
                if (this.MainGridTranslateTransform.Y < (-(this.MainGrid.ActualHeight - this.ActualHeight) - this.margin))
                {
                    this.MainGridTranslateTransform.Y = (-(this.MainGrid.ActualHeight - this.ActualHeight) - this.margin);
                }
                if (this.MainGridTranslateTransform.Y > this.margin)
                {
                    this.MainGridTranslateTransform.Y = this.margin;
                }
            }
        }
        e.Handled = true;
    }

    private Point previousPosition;
    private double margin = 200;

    private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
    {
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
        this.MainGridTranslateTransform.X += (currentPosition.X - this.previousPosition.X);
        this.MainGridTranslateTransform.Y += (currentPosition.Y - this.previousPosition.Y) * 2;
        this.previousPosition = e.GetPosition(this);
        if (this.MainGridTranslateTransform.X < (-(this.MainGrid.ActualWidth - this.ActualWidth) - this.margin))
        {
            this.MainGridTranslateTransform.X = (-(this.MainGrid.ActualWidth - this.ActualWidth)  - this.margin) ;
        }
        if (this.MainGridTranslateTransform.X > this.margin)
        {
            this.MainGridTranslateTransform.X = this.margin;
        }
        if (this.MainGridTranslateTransform.Y < (-(this.MainGrid.ActualHeight - this.ActualHeight) - this.margin))
        {
            this.MainGridTranslateTransform.Y = (-(this.MainGrid.ActualHeight - this.ActualHeight) - this.margin) ;
        }
        if (this.MainGridTranslateTransform.Y > this.margin)
        {
            this.MainGridTranslateTransform.Y = this.margin;
        }
    }
}
