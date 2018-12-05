using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DataStorage.Models;
using SimpleReminder.Tools;
using Tools;

namespace SimpleReminder.ViewModels
{
    class EditorViewModel : INotifyPropertyChanged
    {
        #region Fields

        private ReminderData _editedValue;
        private DateTime _selectedDate;
        private int _hours;
        private int _minutes;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                BuildNewDate();
                OnPropertyChanged();
            }
        }

        public int Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                BuildNewDate();
                OnPropertyChanged();
            }
        }

        public int Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                BuildNewDate();
                OnPropertyChanged();
            }
        }

        public string ReminderText
        {
            get { return _editedValue.ReminderText; }
            set
            {
                _editedValue.ReminderText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private ICommand _saveCommand;
        private ICommand _deleteCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                       (_saveCommand = new RelayCommand<object>(Save, CanSave));
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??
                       (_deleteCommand = new RelayCommand<object>(Delete));
            }
        }

        #endregion

        public EditorViewModel(ReminderData data)
        {
            _editedValue = data;
            _selectedDate = data.SelectedDate;
            _hours = data.SelectedDate.Hour;
            _minutes = data.SelectedDate.Minute;
        }

        private void BuildNewDate()
        {
            _editedValue.SelectedDate = new DateTime(_selectedDate.Year, _selectedDate.Month, _selectedDate.Day, _hours, _minutes, 0);
        }

        #region Command Implementation

        private void Save(object arg)
        {
            OnSaveRequire?.Invoke(_editedValue);
        }

        private bool CanSave(object arg)
        {
            return _editedValue.SelectedDate > DateTime.Now;
        }

        private void Delete(object arg)
        {
            OnDeleteRequire?.Invoke(_editedValue);
        }

        #endregion

        public event Delegates.ActionWithReminderRequired OnSaveRequire;
        public event Delegates.ActionWithReminderRequired OnDeleteRequire;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
