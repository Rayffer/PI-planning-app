using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPlanningApp.Models
{
    internal class IterationFeatureSlot : INotifyPropertyChanged
    {
        private Guid parentFeatureId;
        private Guid parentIterationId;
        private int rowPosition;
        private int columnPosition;


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

        public int RowPosition
        {
            get => this.rowPosition;
            set
            {
                this.rowPosition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RowPosition)));
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

        public ObservableCollection<UserStory> UserStories { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RemoveUserStory(UserStory userStory)
        {
            this.UserStories.Remove(userStory);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UserStories)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UserStories.Count)));
        }

        public void AddNewUserStory(UserStory userStory)
        {
            this.UserStories.Add(userStory);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UserStories)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UserStories.Count)));
        }
    }
}
