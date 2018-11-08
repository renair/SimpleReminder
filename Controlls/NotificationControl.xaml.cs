using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SimpleReminder.Data;

namespace SimpleReminder.Controlls
{
    /// <summary>
    /// Логика взаимодействия для Notification.xaml
    /// </summary>
    public partial class NotificationControl
    {
        public delegate void PassingData(ReminderData d);
        public delegate void ObjectChanged(NotificationControl n);

        // This event will be invoked when user click on notificatioin
        public event PassingData RequiringSettings;
        // This event will be invoked when notification became outdated.
        // Checking for out dating performs in Update() method.
        public event ObjectChanged NotificationOutdated;
        // This event will be invoked when notifaction already outdated
        // and user click on it.
        public event ObjectChanged ReadyToRemove;

        private ReminderData _reminderData;

        public ReminderData ReminderData
        {
            get => _reminderData;
            set
            {
                _reminderData = value;
                // Display setted data
                DisplayDate();
            }
        }

        public NotificationControl(ReminderData data)
        {
            InitializeComponent();
            _reminderData = data;
            // Display data on object initialization.
            DisplayDate();
        }

        public void DisplayDate()
        {
            DateTime t = _reminderData.SelectedDate;
            DateLabel.Content = t.ToShortDateString();
            HoursLabel.Content = t.Hour.ToString().PadLeft(2, '0');
            MinutesLabel.Content = t.Minute.ToString().PadLeft(2, '0');
            NotificationMessage.Text = _reminderData.ReminderText;
            Update();
        }
        
        public void Update()
        {
            // This method will work only if notification just became outdated
            // Checking brush color do as marker is outdated event already invoked
            // and to make oudated event red.
            if(_reminderData.IsTimeCome() && MainButton.Background != Brushes.Red)
            {
                MainButton.Background = Brushes.Red;
                NotificationOutdated?.Invoke(this);
            }
            // If we edit notification when it already outdated we will change
            // data in it, so we needto be able to change notification status
            else if(!_reminderData.IsTimeCome())
            {
                MainButton.Background = Brushes.White;
            }
        }

        private void MainButtonClick(object sender, RoutedEventArgs e)
        {
            // Check is reminder is outdated and it it is - invoke ReadyToRemove
            // in other way - it requre settings for this reminder.
            if(_reminderData.IsTimeCome())
            {
                ReadyToRemove?.Invoke(this);
                return;
            }
            RequiringSettings?.Invoke(_reminderData);
        }

        // TODO Remove method
        // This method was writed for tests. You can press left button on notification
        // and it immediately became oudated.
        private void MainButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton.HasFlag(MouseButtonState.Pressed))
            {
                _reminderData.SelectedDate = DateTime.Now;
                DisplayDate();
            }
            e.Handled = true;
        }
    }
}
