using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataStorage.Models;
using SimpleReminder.Controlls;
using SimpleReminder.Managers;
using SimpleReminder.Screens;
using Tools;

namespace SimpleReminder.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        private UIElementCollection _notifications;
        private Visibility _editorVisibility = Visibility.Hidden;
        private ReminderData _editedValue;
        private NotificationScreen _editorUi;

        public Visibility EditorVisibility
        {
            get { return _editorVisibility; }
            set
            {
                _editorVisibility = value;
                OnPropertyChanged();
            }
        }

        public ReminderData EditedValue
        {
            get { return _editedValue; }
            set
            {
                _editedValue = value;
                OnPropertyChanged();
            }
        }

        public NotificationScreen EditorUi
        {
            get { return _editorUi ?? (_editorUi = new NotificationScreen()); }
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

        public MainViewModel(UIElementCollection notificationCollection)
        {
            _notifications = notificationCollection;
            FillNotifications();
        }

        private void FillNotifications()
        {
            _notifications.Clear();
            foreach (var notification in AccountManager.CurrentUser.Notifications)
            {
                NotificationControl ctrl = new NotificationControl(notification);
                NotificationViewModel nvm = new NotificationViewModel(notification);
                nvm.OnRequireConfiguration += OpenEditor;
                nvm.OnRequireDeletion += RemoveReminding;
                ctrl.DataContext = nvm;
                _notifications.Add(ctrl);
            }
        }

        private void OpenEditor(ReminderData data)
        {
            EditorVisibility = Visibility.Visible;
            //EditedValue = data;
            EditorUi.DataContext = new EditorViewModel(data);
        }

        private async void RemoveReminding(ReminderData data)
        {
            LoaderManager.ShowLoader();
            await AccountManager.DeleteReminding(data);
            FillNotifications();
            LoaderManager.HideLoader();
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
