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
        private ReminderData reminderData;

        public NotificationControll(ReminderData data)
        {
            InitializeComponent();
            InitializeTimePickers();
            reminderData = data;
            DatePicker.DisplayDateStart = DateTime.Now;
            Update();
        }

        private void InitializeTimePickers()
        {
            for(int i = 0; i <= 60; ++i)
            {
                string s = i.ToString();
                if(i <= 24)
                {
                    HoursPicker.Items.Add(s.PadLeft(2, '0'));
                }
                MinutesPicker.Items.Add(s.PadLeft(2, '0'));
            }
        }

        public void Update()
        {
            DatePicker.SelectedDate = reminderData.SelectedDate;
            HoursPicker.SelectedIndex = reminderData.SelectedDate.Hour;
            MinutesPicker.SelectedIndex = reminderData.SelectedDate.Minute;
            NotificationMessage.Text = reminderData.ReminderText;
        }
    }
}
