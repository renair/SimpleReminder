using System;
using System.Threading.Tasks;
using DataStorage.DataAccess;
using DataStorage.Models;
using Tools;
using System.Configuration;

namespace SimpleReminder.Managers
{
    static class AccountManager
    {
        private static UserData _currentUser;
        private static IDataAccess _dataAccessor;

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
            _dataAccessor = new WebAccessor(ConfigurationManager.AppSettings["ServerHost"]);
            try
            {
                userCandidate = Serializer.Deserialize<UserData>(FileFolderHelper.LastUserFilePath);
                LoaderManager.ShowLoader();
                var userNotifications = _dataAccessor.GetUserNotifications(userCandidate.Id);
                if (userNotifications == null)
                {
                    Logger.Log("Remote API don't response");
                    CurrentUser = null;
                    return;
                }
                userCandidate.Notifications = userNotifications;
            }
            catch (Exception ex)
            {
                userCandidate = null;
                Logger.Log("Failed to deserialize last user", ex);
            }
            finally
            {
                LoaderManager.HideLoader();
            }
            CurrentUser = userCandidate;
        }

        public static Task<bool> SignIn(string login, string password)
        {
            return Task.Run(() =>
            {
                UserData data = _dataAccessor.SignIn(login, password);
                if (data != null)
                {
                    CurrentUser = data;
                    return true;
                }
                return false;
            });
        }

        public static Task<bool> SignUp(UserData data)
        {
            return Task.Run(() =>
            {
                UserData signedUser = _dataAccessor.SignUp(data);
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
            return Task.Run(() => _dataAccessor.AddNotification(data));
        }

        // Simulate REST api call
        public static Task<bool> DeleteReminding(ReminderData data)
        {
            return Task.Run(() => _dataAccessor.RemoveNotification(data));
        }

        // Simulate REST api call
        public static Task<bool> UpdateReminding(ReminderData data)
        {
            return Task.Run(() => _dataAccessor.UpdateNotification(data));
        }
    }
}
