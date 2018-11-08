using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleReminder.Data;

namespace SimpleReminder.DataAccess
{
    // Just test case. In future will replace with DB or remote
    // API equests.
    static class UserAccess
    {
        // Hardcoded user which used as stub to log in system.
        private static List<UserData> users = new List<UserData>() {
            new UserData{Id = 1,  Surname = "Gomenyuk", Name = "Andrew", Login = "andrew", Email = "mail@m.com", LastLogin = DateTime.Now, PasswordHash = GetHash("andrew")}
        };

        // Method to get has as string from string.
        // Writed for hashing passwords.
        private static string GetHash(string s)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder builder = new StringBuilder();
            foreach(byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }

        // Check if thre is account with given login and password
        public static Task<bool> IsRegistered(string login, string passwd)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(5*1000);
                foreach (UserData ud in users)
                {
                    if (ud.Login == login && ud.PasswordHash == GetHash(passwd))
                    {
                        return true;
                    }
                }

                return false;
            });
        }
    }
}
