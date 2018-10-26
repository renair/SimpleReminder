using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleReminder.Data
{
    class UserData
    {
        private UInt64 id;
        private string surname;
        private string name;
        private string login;
        private string email;
        private string passwordHash;
        private DateTime lastLogin;

        public UInt64 Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }

        public string PasswordHash
        {
            get
            {
                return passwordHash;
            }
            set
            {
                passwordHash = value;
            }
        }

        public DateTime LastLogin
        {
            get
            {
                return lastLogin;
            }
            set
            {
                lastLogin = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
    }
}
