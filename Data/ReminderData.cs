using System;
using System.Collections.Generic;

namespace SimpleReminder.Data
{
    public class ReminderData : IComparer<ReminderData>
    {
        // Private parts with data
        private Int64 id;
        private DateTime selectedDate;
        private string reminderText;

        // Properties for private fields
        public Int64 Id
        {
            get
            {
                return id;
            }
            private set
            {
                id = value;
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                selectedDate = value;
            }
        }

        public string ReminderText
        {
            get
            {
                return reminderText;
            }
            set
            {
                reminderText = value;
            }
        }

        public int Compare(ReminderData a, ReminderData b)
        {
            return (int)(a.selectedDate - b.selectedDate).TotalMinutes;
        }

        public bool isTimeCome()
        {
            return selectedDate <= DateTime.Now;
        }
    }
}
