using System;

namespace SimpleReminder.Data
{
    public class ReminderData : IComparable
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

        public int CompareTo(object obj)
        {
            if (obj != null && obj is ReminderData)
            {
                ReminderData r = (ReminderData)obj;
                return (int)(this.selectedDate - r.selectedDate).TotalMinutes;
            }
            throw new Exception();
        }

        public bool isTimeCome()
        {
            return selectedDate <= DateTime.Now;
        }
    }
}
