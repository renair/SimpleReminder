using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DataStorage.Models;

namespace SimpleReminder.ViewModels
{
    class EditorViewModel : INotifyPropertyChanged
    {
        #region Fields

        private DateTime _selectedDate;
        private int _hours;
        private int _minutes;
        private string _reminderText;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                _hours = value.Hour;
                _minutes = value.Minute;
                OnPropertyChanged();
            }
        }

        public int Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged();
            }
        }

        public int Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                OnPropertyChanged();
            }
        }

        public string ReminderText
        {
            get { return _reminderText; }
            set
            {
                _reminderText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public EditorViewModel(ReminderData data)
        {
            SelectedDate = data.SelectedDate;
            ReminderText = data.ReminderText;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
