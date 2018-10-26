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

namespace SimpleReminder.Screens
{
    /// <summary>
    /// Логика взаимодействия для NotificationScreen.xaml
    /// </summary>
    public partial class NotificationScreen : UserControl
    {
        private ReminderData reminderData;
        private DateTime selectedDateTime;

        public ReminderData ReminderData
        {
            get
            {
                return reminderData;
            }

            set
            {
                reminderData = value;
                UpdateWithData();
            }
        }

        public delegate void ObjChanged(NotificationScreen s);

        public event ObjChanged SaveRequired;
        public event ObjChanged RemoveRequired;

        public NotificationScreen(ReminderData data)
        {
            InitializeComponent();
            InitializeTimePickers();
            ReminderData = data;
            selectedDateTime = data.SelectedDate;
        }

        private void InitializeTimePickers()
        {
            for (int i = 0; i < 60; ++i)
            {
                string s = i.ToString();
                if (i < 24)
                {
                    HoursPicker.Items.Add(s.PadLeft(2, '0'));
                }
                MinutesPicker.Items.Add(s.PadLeft(2, '0'));
            }
        }

        private void UpdateWithData()
        {
            DatePicker.SelectedDate = ReminderData.SelectedDate;
            HoursPicker.SelectedIndex = ReminderData.SelectedDate.Hour;
            MinutesPicker.SelectedIndex = ReminderData.SelectedDate.Minute;
            NotificationMessage.Text = ReminderData.ReminderText;
        }

        private void DatePickerSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime s = selectedDateTime;
            if (DatePicker.SelectedDate.HasValue && (DatePicker.SelectedDate.Value - DateTime.Now).TotalDays > -1)
            {
                DateTime p = DatePicker.SelectedDate.Value;
                selectedDateTime = new DateTime(p.Year, p.Month, p.Day, s.Hour, s.Minute, 0);
            }
        }

        private void HoursPickerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime s = selectedDateTime;
            selectedDateTime = new DateTime(s.Year, s.Month, s.Day, HoursPicker.SelectedIndex, s.Minute, 0);
        }

        private void MinutesPickerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime s = selectedDateTime;
            selectedDateTime = new DateTime(s.Year, s.Month, s.Day, s.Hour, MinutesPicker.SelectedIndex, 0);
        }

        private void NotificationMessageTextChanged(object sender, TextChangedEventArgs e)
        {
            reminderData.ReminderText = NotificationMessage.Text;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (selectedDateTime > DateTime.Now)
            {
                ReminderData.SelectedDate = selectedDateTime;
                SaveRequired?.Invoke(this);
            }
            else
            {
                MessageBox.Show("Selected date and time shoud be in future!");
            }
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            RemoveRequired?.Invoke(this);
        }
    }
}
