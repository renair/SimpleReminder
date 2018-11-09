using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleReminder.Data;
using SimpleReminder.DataAccess;
using SimpleReminder.Tools;

namespace SimpleReminder.Managers
{
    static class AccountManager
    {
        private static UserData _currentUser;

        public static UserData CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                if (value == null)
                {
                    FileFolderHelper.RemoveLastUserCache();
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

        // Simulate REST api call
        public static Task<bool> AddReminding(ReminderData data)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(250);
                CurrentUser.Notifications.Add(data);
                try
                {
                    Serializer.Serialize(_currentUser, FileFolderHelper.LastUserFilePath);
                    UserAccess.UpdateDataFile();
                }
                catch (Exception e)
                {
                    Logger.Log("Can't serialize current user", e);
                }
                return true;
            });
        }

        // Simulate REST api call
        public static Task<bool> DeleteReminding(ReminderData data)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(250);
                bool result = CurrentUser.Notifications.Remove(data);
                try
                {
                    Serializer.Serialize(_currentUser, FileFolderHelper.LastUserFilePath);
                    UserAccess.UpdateDataFile();
                }
                catch (Exception e)
                {
                    Logger.Log("Can't serialize current user", e);
                }
                return result;
            });
        }

        // Simulate REST api call
        public static Task<bool> UpdateReminding(ReminderData data)
        {
            // Do nothing here, because data will be updated via reference
            // This method ONLY for future using with REST api
            return Task.Run(() =>
            {
                Thread.Sleep(250);
                try
                {
                    Serializer.Serialize(_currentUser, FileFolderHelper.LastUserFilePath);
                    UserAccess.UpdateDataFile();
                }
                catch (Exception e)
                {
                    Logger.Log("Can't serialize current user", e);
                }
                return true;
            });
        }
    }
}
