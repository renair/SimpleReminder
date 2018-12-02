using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DataStorage.Models;
using SimpleReminder.Controlls;
using SimpleReminder.Managers;
using Tools;

namespace SimpleReminder.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<NotificationControl> _notifications;
        
        public ObservableCollection<NotificationControl> Notifications
        {
            get { return _notifications; }
        }
        #endregion

        #region Commands
        private ICommand _addRemindingCommand;
        private ICommand _logOutCommand;

        public ICommand AddRemindingCommand
        {
            get
            {
                return _addRemindingCommand ?? (_addRemindingCommand = new RelayCommand<object>(AddReminding));
            }
        }

        public ICommand LogOutCommand
        {
            get
            {
                return _logOutCommand ?? (_logOutCommand = new RelayCommand<object>(LogOut));
            }
        }
        #endregion

        #region Methods
        private void FillNotifications()
        {
            _notifications = new ObservableCollection<NotificationControl>();
            foreach (var notification in AccountManager.CurrentUser.Notifications)
            {
                _notifications.Add(new NotificationControl(notification));
            }
        }
        #endregion

        #region CommandImplementation
        private async void AddReminding(object arg)
        {
            ReminderData data = new ReminderData(AccountManager.CurrentUser)
            {
                SelectedDate = DateTime.Now.AddHours(1),
                ReminderText = ""
            };

            LoaderManager.ShowLoader();
            await AccountManager.AddReminding(data);
            LoaderManager.HideLoader();

            NotificationControl ctrl = new NotificationControl(data);
            _notifications.Add(ctrl);
        }

        private async void LogOut(object arg)
        {
            await AccountManager.LogOut();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
