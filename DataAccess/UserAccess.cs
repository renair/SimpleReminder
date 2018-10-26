using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleReminder.Data;

namespace SimpleReminder.DataAccess
{
    // Just test case. In future will replace with DB or remote
    // API equests.
    class UserAccess
    {
        private static List<UserData> users = new List<UserData>() {
            new UserData{Id = 1,  Surname = "Gomenyuk", Name = "Andrew", Login = "andrew", Email = "mail@m.com", LastLogin = DateTime.Now, PasswordHash = GetHash("andrew")}
        };

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

        public static bool IsRegistered(string login, string passwd)
        {
            foreach(UserData ud in users)
            {
                if(ud.Login == login && ud.PasswordHash == GetHash(passwd))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
