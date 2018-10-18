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

        // Make initialization to safe using without NullReferenceException.
        public ReminderData()
        {
            selectedDate = DateTime.Now;
        }

        public ReminderData(Int64 id)
        {
            selectedDate = DateTime.Now;
            this.id = id;
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
