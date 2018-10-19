using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public event PassingData RequiringSettings;
        public event ObjectChanged NotificationOutdated;
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
                DisplayDate();
            }
        }

        public NotificationControll(ReminderData data)
        {
            InitializeComponent();
            reminderData = data;
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
            //TODO replace it with normal code
            if(reminderData.isTimeCome() && MainButton.Background != Brushes.Red)
            {
                MainButton.Background = Brushes.Red;
                NotificationOutdated?.Invoke(this);
            }
        }

        private void MainButtonClick(object sender, RoutedEventArgs e)
        {
            if(reminderData.isTimeCome())
            {
                ReadyToRemove?.Invoke(this);
                return;
            }
            RequiringSettings?.Invoke(reminderData);
        }

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
