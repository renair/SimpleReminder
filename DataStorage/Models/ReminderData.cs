using System;

namespace DataStorage.Models
{
    [Serializable]
    public class ReminderData : IComparable
    {
        private UserData _user;
        
        public long Id { get; private set; }

        public DateTime SelectedDate { get; set; }

        public string ReminderText { get; set; }

        public Int64 UserId { get; internal set; }

        public UserData User
        {
            get { return _user; }
            set
            {
                _user = value;
                if (value != null)
                {
                    UserId = value.Id;
                }
            }
        }
        
        public ReminderData(UserData user, Int64 id = 0)
        {
            Id = id;
            SelectedDate = DateTime.Now;
            User = user;
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
