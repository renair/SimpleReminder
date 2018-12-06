using System;
using System.Collections.Generic;
using Tools;

namespace DataStorage.Models
{
    [Serializable]
    public class UserData
    {
        private List<ReminderData> _notifications;
        public Int64 Id { get; private set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; internal set; }

        public string Password
        {
            set
            {
                PasswordHash = Encryption.GetHash(value);
            }
        }

        public DateTime LastLogin { get; set; }

        public List<ReminderData> Notifications
        {
            get
            {

                _notifications.Sort((a, b) => a.SelectedDate.CompareTo(b.SelectedDate));
                return _notifications;
            }
            set { _notifications = value; }
        }

        public string Email { get; set; }

        public UserData(Int64 id = 0)
        {
            Id = id;
            Notifications = new List<ReminderData>();
        }
    }
}
