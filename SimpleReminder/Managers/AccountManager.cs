using System;
using System.Threading.Tasks;
using DataStorage.DataAccess;
using DataStorage.Models;
using Tools;

namespace SimpleReminder.Managers
{
    static class AccountManager
    {
        private static UserData _currentUser;

        public static UserData CurrentUser
        {
            get { return _currentUser; }
            private set
            {
                _currentUser = value;
                if (value == null)
                {
                    FileFolderHelper.RemoveLastUserCache();
                    return;
                }
                try
                {
                    Serializer.Serialize(_currentUser, FileFolderHelper.LastUserFilePath);
                }
                catch (Exception e)
                {
                    Logger.Log("Can't serialize current user", e);
                }
            }
        }

        static AccountManager()
        {
            UserData userCandidate;
            try
            {
                userCandidate = Serializer.Deserialize<UserData>(FileFolderHelper.LastUserFilePath);
            }
            catch (Exception ex)
            {
                userCandidate = null;
                Logger.Log("Failed to deserialize last user", ex);
            }
            _currentUser = userCandidate;
        }

        public static Task<bool> SignIn(string login, string password)
        {
            return Task.Run(() =>
            {
                UserData data = DbAccessor.SignIn(login, password);
                if (data != null)
                {
                    CurrentUser = data;
                    return true;
                }
                return false;
            });
        }

        public static Task<bool> SignUp(UserData data, string password)
        {
            return Task.Run(() =>
            {
                UserData signedUser = DbAccessor.SignUp(data, password);
                if (signedUser != null)
                {
                    CurrentUser = signedUser;
                    return true;
                }
                return false;
            });
        }

        public static Task LogOut()
        {
            return Task.Run(() => { CurrentUser = null; });
        }

        // Simulate REST api call
        public static Task<bool> AddReminding(ReminderData data)
        {
            return Task.Run(() => DbAccessor.AddNotification(data));
        }

        // Simulate REST api call
        public static Task<bool> DeleteReminding(ReminderData data)
        {
            return Task.Run(() => DbAccessor.RemoveNotification(data));
        }

        // Simulate REST api call
        public static Task<bool> UpdateReminding(ReminderData data)
        {
            return Task.Run(() => DbAccessor.UpdateNotification(data));
        }
    }
}
