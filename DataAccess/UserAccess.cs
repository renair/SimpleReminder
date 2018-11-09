using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleReminder.Data;
using SimpleReminder.Managers;
using SimpleReminder.Tools;

namespace SimpleReminder.DataAccess
{
    // Just test case. In future will replace with DB or remote
    // API equests.
    static class UserAccess
    {
        // Hardcoded user which used as stub to log in system.
        private static List<UserData> _users = new List<UserData>() {
            new UserData{Surname = "Gomenyuk", Name = "Andrew", Login = "andrew", Email = "mail@m.com", LastLogin = DateTime.Now, PasswordHash = GetHash("andrew")}
        };

        static UserAccess()
        {
            List<UserData> usersCandidate = null;
            try
            {
                usersCandidate = Serializer.Deserialize<List<UserData>>(FileFolderHelper.StorageFilePath);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to deserialize user list.", ex);
            }

            if (usersCandidate != null)
            {
                _users = usersCandidate;
                Logger.Log("Users successfully deserialized");
            }
        }

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
        public static Task<bool> SignIn(string login, string passwd)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(2 * 1000);
                foreach (UserData ud in _users)
                {
                    if (ud.Login == login && ud.PasswordHash == GetHash(passwd))
                    {
                        ud.LastLogin = DateTime.Now;
                        AccountManager.CurrentUser = ud;
                        UpdateDataFile();
                        return true;
                    }
                }

                return false;
            });
        }

        public static Task<UserData> SignUp(UserData userData, string password)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(3 * 1000);
                // Fetching last id
                ulong lastId = _users.OrderBy(u => u.Id).Last().Id;
                // Init new user with id and hashed password
                UserData dataWithId = new UserData(lastId, userData);
                dataWithId.PasswordHash = GetHash(password);
                // Add to global users storage and update storage
                _users.Add(dataWithId);
                UpdateDataFile();
                return dataWithId;
            });
        }

        // public access to update from account manager
        // it will be removed in future
        public static void UpdateDataFile()
        {
            try
            {
                Serializer.Serialize(_users, FileFolderHelper.StorageFilePath);
            }
            catch (Exception e)
            {
                Logger.Log("Can't serialize users too file.", e);
            }
        }
    }
}
