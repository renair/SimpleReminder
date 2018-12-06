using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using DataStorage.Models;
using SimpleReminder.Tools;
using Tools;

namespace SimpleReminder.ViewModels
{
    class NotificationViewModel : INotifyPropertyChanged
    {
        #region Fields

        private ReminderData _reminderData;

        public ReminderData ContainedData
        {
            get { return _reminderData; }
            private set
            {
                _reminderData = value;
                OnPropertyChanged();
            }
        }

        public string Date
        {
            get { return _reminderData.SelectedDate.ToShortDateString(); }
        }

        public string Hours
        {
            get { return _reminderData.SelectedDate.Hour.ToString().PadLeft(2, '0'); }
        }

        public string Minutes
        {
            get { return _reminderData.SelectedDate.Minute.ToString().PadLeft(2, '0'); }
        }

        public Brush Color
        {
            get { return _reminderData.SelectedDate >= DateTime.Now ? Brushes.Transparent : Brushes.Red; }
        }

        public string Text
        {
            get { return _reminderData.ReminderText; }
        }
        #endregion

        #region Commands

        private ICommand _requireConfiguration;

        public ICommand RequireConfigutationCommand
        {
            get
            {
                return _requireConfiguration ??
                       (_requireConfiguration = new RelayCommand<object>(RequireConfigurationInvoker));
            }
        }

        #endregion

        public NotificationViewModel(ReminderData data)
        {
            ContainedData = data;
            if (!data.IsTimeCome())
            {
                // This will trigger property changed and UI will update
                // and update notification color.

                // ReSharper disable once ExplicitCallerInfoArgument
                Task.Delay(data.SelectedDate - DateTime.Now).ContinueWith(task => OnPropertyChanged("Color"));
            }
        }

        private void RequireConfigurationInvoker(object obj)
        {
            if(ContainedData.SelectedDate <= DateTime.Now)
            {
                OnRequireDeletion?.Invoke(ContainedData);
                return;
            }
            OnRequireConfiguration?.Invoke(ContainedData);
        }

        public event Delegates.ActionWithReminderRequired OnRequireConfiguration;
        public event Delegates.ActionWithReminderRequired OnRequireDeletion;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
