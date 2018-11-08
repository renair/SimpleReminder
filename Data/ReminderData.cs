using System;

namespace SimpleReminder.Data
{
    public class ReminderData : IComparable
    {
        // Private parts with data

        // Properties for private fields
        public long Id { get; private set; }

        public DateTime SelectedDate { get; set; }

        public string ReminderText { get; set; }

        // Make initialization to safe using without NullReferenceException.
        public ReminderData()
        {
            SelectedDate = DateTime.Now;
        }

        public ReminderData(Int64 id)
        {
            SelectedDate = DateTime.Now;
            Id = id;
        }

        public int CompareTo(object obj)
        {
            if (obj != null && obj is ReminderData)
            {
                ReminderData r = (ReminderData)obj;
                return (int)Math.Ceiling((SelectedDate - r.SelectedDate).TotalMinutes + 0.5);
            }
            throw new Exception();
        }

        public bool IsTimeCome()
        {
            return SelectedDate <= DateTime.Now;
        }
    }
}
