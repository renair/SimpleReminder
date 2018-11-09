using System;
using System.Collections.Generic;

namespace SimpleReminder.Data
{
    [Serializable]
    internal class UserData
    {
        public ulong Id { get; private set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public DateTime LastLogin { get; set; }

        public List<ReminderData> Notifications { get; set; }

        public string Email { get; set; }

        public UserData()
        {
            Notifications = new List<ReminderData>();
        }

        public UserData(ulong id, UserData copySrc = null)
        {
            Id = id;
            if (copySrc == null) return;
            Surname = copySrc.Surname;
            Name = copySrc.Name;
            Login = copySrc.Login;
            PasswordHash = copySrc.PasswordHash;
            LastLogin = copySrc.LastLogin;
            Email = copySrc.Email;
            Notifications = copySrc.Notifications;
        }
    }

    
}
