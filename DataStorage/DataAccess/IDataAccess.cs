using System;
using System.Collections.Generic;
using DataStorage.Models;

namespace DataStorage.DataAccess
{
    public interface  IDataAccess
    {
        UserData SignIn(string login, string passwd);
        List<ReminderData> GetUserNotifications(Int64 userId);
        UserData SignUp(UserData userData, string password);
        bool AddNotification(ReminderData data);
        bool RemoveNotification(ReminderData data);
        bool UpdateNotification(ReminderData data);
    }
}
