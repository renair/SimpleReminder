using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DataStorage.Models;
using Tools;

namespace DataStorage.DataAccess
{
    // Just test case. In future will replace with DB or remote
    // API equests.
    public static class DbAccessor
    {
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
        public static UserData SignIn(string login, string passwd)
        {
            using (var db = new ReminderDBContext())
            {
                try
                {
                    string hashedPassword = GetHash(passwd);
                    return db.Users.Include(u => u.Notifications).FirstOrDefault(u => u.Login == login && u.PasswordHash == hashedPassword);
                }
                catch (Exception e)
                {
                    Logger.Log("Can't log in into account!", e);
                    return null;
                }
            }
        }

        public static List<ReminderData> GetUserNotifications(Int64 userId)
        {
            using (var db = new ReminderDBContext())
            {
                try
                {
                    return db.Remindings.Where(r => r.UserId == userId).ToList();
                }
                catch (Exception e)
                {
                    Logger.Log("Can't get user notifications!", e);
                    return null;
                }
            }
        }

        public static UserData SignUp(UserData userData, string password)
        {
            using (var db = new ReminderDBContext())
            {
                try
                {
                    userData.PasswordHash = GetHash(password);

                    if (db.Users.Any(u => u.Login == userData.Login))
                    {
                        return null;
                    }

                    UserData dataFromDb = db.Users.Add(userData);
                    db.SaveChanges();
                    return dataFromDb;
                }
                catch (Exception e)
                {
                    Logger.Log("Can't save user id DB", e);
                    return null;
                }
            }
        }

        public static bool AddNotification(ReminderData data)
        {
            using (var db = new ReminderDBContext())
            {
                try
                {
                    UserData attachedUser = data.User;
                    data.User = null;
                    db.Remindings.Add(data);
                    db.SaveChanges();
                    data.User = attachedUser;
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Log("Can't add notification.", e);
                    return false;
                }
            }
        }

        public static bool RemoveNotification(ReminderData data)
        {
            using (var db = new ReminderDBContext())
            {
                try
                {
                    UserData attachedUser = data.User;
                    data.User = null;
                    db.Entry(data).State = EntityState.Deleted;
                    db.SaveChanges();
                    data.User = attachedUser;
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Log("Can't remove notification.", e);
                    return false;
                }
            }
        }

        public static bool UpdateNotification(ReminderData data)
        {
            using (var db = new ReminderDBContext())
            {
                try
                {
                    UserData attachedUser = data.User;
                    data.User = null;
                    db.Remindings.AddOrUpdate(data);
                    db.SaveChanges();
                    data.User = attachedUser;
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Log("Can't update notification.", e);
                    return false;
                }
            }
        }
    }
}
