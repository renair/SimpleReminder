using System;

namespace SimpleReminder.Data
{
    internal class UserData
    {
        public ulong Id { get; internal set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public DateTime LastLogin { get; set; }

        public string Email { get; set; }
    }
}
