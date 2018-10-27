using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SimpleReminder.Data;

namespace SimpleReminder.Controlls
{
    /// <summary>
    /// Логика взаимодействия для Notification.xaml
    /// </summary>
    public partial class NotificationControll : UserControl
    {
        public delegate void PassingData(ReminderData d);
        public delegate void ObjectChanged(NotificationControll n);

        // This event will be invoked when user click on notificatioin
        public event PassingData RequiringSettings;
        // This event will be invoked when notification became outdated.
        // Checking for outdating performs in Update() method.
        public event ObjectChanged NotificationOutdated;
        // This event will be invoked when notifaction already outdated
        // and user click on it.
        public event ObjectChanged ReadyToRemove;

        private ReminderData reminderData;

        public ReminderData ReminderData
        {
            get
            {
                return reminderData;
            }
            set
            {
                reminderData = value;
                // Display setted data
                DisplayDate();
            }
        }

        public NotificationControll(ReminderData data)
        {
            InitializeComponent();
            reminderData = data;
            // Display data on object initialization.
            DisplayDate();
        }

        public void DisplayDate()
        {
            DateTime t = reminderData.SelectedDate;
            DateLabel.Content = t.ToShortDateString();
            HoursLabel.Content = t.Hour.ToString().PadLeft(2, '0');
            MinutesLabel.Content = t.Minute.ToString().PadLeft(2, '0');
            NotificationMessage.Text = reminderData.ReminderText;
            Update();
        }
        
        public void Update()
        {
            // This method will work only if notification just became outdated
            // Checking brush color do as marker is outdated event already invoked
            // and to make oudated event red.
            if(reminderData.isTimeCome() && MainButton.Background != Brushes.Red)
            {
                MainButton.Background = Brushes.Red;
                NotificationOutdated?.Invoke(this);
            }
            // If we edit notification when it already outdated we will change
            // data in it, so we needto be able to change notification status
            else if(!reminderData.isTimeCome())
            {
                MainButton.Background = Brushes.White;
            }
        }

        private void MainButtonClick(object sender, RoutedEventArgs e)
        {
            // Check is reminder is outdated and it it is - invoke ReadyToRemove
            // in other way - it requre settings for this reminder.
            if(reminderData.isTimeCome())
            {
                ReadyToRemove?.Invoke(this);
                return;
            }
            RequiringSettings?.Invoke(reminderData);
        }

        // TODO Remove method
        // This method was writed for tests. You can press left button on notification
        // and it immediately became oudated.
        private void MainButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton.HasFlag(MouseButtonState.Pressed))
            {
                reminderData.SelectedDate = DateTime.Now;
                DisplayDate();
            }
            e.Handled = true;
        }
    }
}
