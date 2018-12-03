using System;
using System.Windows;
using System.Windows.Controls;
using DataStorage.Models;

namespace SimpleReminder.Screens
{
    public partial class NotificationScreen
    {
        private ReminderData _reminderData;

        public ReminderData ReminderData
        {
            get
            {
                return _reminderData;
            }

            set
            {
                _reminderData = value;
                Update();
            }
        }

        public delegate void DataChanged(ReminderData s);

        // This event will be invoked when SaveButton pressed
        public event DataChanged SaveRequired;
        // This event will be invoked when RemoveButton pressed
        public event DataChanged RemoveRequired;

        public NotificationScreen()//ReminderData data)
        {
            InitializeComponent();
            InitializeTimePickers();
            //ReminderData = data;
        }

        // This method initialize ComboBox with hours and minutes
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

        // This method is called when smbdy set new ReminderData
        private void Update()
        {
            //DatePicker.SelectedDate = ReminderData.SelectedDate;
            //HoursPicker.SelectedIndex = ReminderData.SelectedDate.Hour;
            //MinutesPicker.SelectedIndex = ReminderData.SelectedDate.Minute;
            //NotificationMessage.Text = ReminderData.ReminderText;
        }

        // Save text whe it was changed
        private void NotificationMessageTextChanged(object sender, TextChangedEventArgs e)
        {
            //_reminderData.ReminderText = NotificationMessage.Text;
        }

        // SaveButton click event handler
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            //// Select date to use
            //DateTime date = DatePicker.SelectedDate.HasValue ? DatePicker.SelectedDate.Value : ReminderData.SelectedDate;
            //// Create new date from currently selected in UI
            //DateTime newDate = new DateTime(date.Year, date.Month, date.Day, HoursPicker.SelectedIndex, MinutesPicker.SelectedIndex, 0);
            //// Check if this date not in the past
            //if (newDate > DateTime.Now)
            //{
            //    // Update SelectedDate with new date
            //    ReminderData.SelectedDate = newDate;
            //    // Invoke saving reuirement
            //    SaveRequired?.Invoke(ReminderData);
            //}
            //else
            //{
            //    MessageBox.Show("Selected date and time shoud be in future!");
            //}
        }

        // SaveButton click event handler
        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            //RemoveRequired?.Invoke(ReminderData);
        }
    }
}
