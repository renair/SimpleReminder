using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DataStorage.Models;
using Tools;

namespace DataStorage.DataAccess
{
    public class WebAccessor : IDataAccess
    {
        private readonly HttpClient _client;

        public WebAccessor(string baseUri)
        { 
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUri);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Check if thre is account with given login and password
        public UserData SignIn(string login, string passwd)
        {
            var authData = new
            {
                Login = login,
                Password = passwd
            };
            try
            {
                HttpResponseMessage response = _client.PostAsJsonAsync("authorise", authData).GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Log("Call 'authorize' endpoint unsuccesfull");
                    return null;
                }
                UserDto userData = response.Content.ReadAsAsync<UserDto>().GetAwaiter().GetResult();
                return userData.ToUserData();
            }
            catch (Exception e)
            {
                Logger.Log("Can't sent authorize request", e);
            }
            return null;
        }

        public UserData SignUp(UserData userData, string password)
        {
            try
            {
                UserDto dto = new UserDto(userData);
                HttpResponseMessage response = _client.PostAsJsonAsync("register", dto).GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Log("Call 'register' endpoint unsuccesfull");
                    return null;
                }
                dto.Id = response.Content.ReadAsAsync<int>().GetAwaiter().GetResult();
                return dto.ToUserData();
            }
            catch (Exception e)
            {
                Logger.Log("Can't sent register request", e);
            }
            return null;
        }

        public List<ReminderData> GetUserNotifications(Int64 userId)
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync($"user_notifications?userId={userId}").GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Log("Call 'user_notification' endpoint unsuccesfull");
                    return null;
                }
                List<NotificationDto> notifications = response.Content.ReadAsAsync<List<NotificationDto>>().GetAwaiter().GetResult();
                List<ReminderData> result = new List<ReminderData>();
                foreach (NotificationDto notification in notifications)
                {
                    result.Add(notification.ToReminderData());
                }
                return result;
            }
            catch (Exception e)
            {
                Logger.Log("Can't retrive user's notifications", e);
            }
            return null;
        }

        public bool AddNotification(ReminderData data)
        {
            try
            {
                NotificationDto dto = new NotificationDto(data);
                HttpResponseMessage response = _client.PostAsJsonAsync("add_notification", dto).GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Log("Call 'add_notification' endpoint unsuccesfull");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Can't sent add notification request", e);
                return false;
            }
        }

        public bool RemoveNotification(ReminderData data)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync($"remove_notification?notificationId={data.Id}").GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Log("Call 'remove_notification' endpoint unsuccesfull");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Can't sent remove notification request", e);
                return false;
            }
        }

        public bool UpdateNotification(ReminderData data)
        {
            try
            {
                NotificationDto dto = new NotificationDto(data);
                HttpResponseMessage response = _client.PostAsJsonAsync("update_notification", dto).GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Log("Call 'update_notification' endpoint unsuccesfull");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Can't sent update_notification request", e);
                return false;
            }
        }

        //-------------------------------------------
        //| DTO (Data Transfer Object) for REST API |
        //|                                         |
        //| All fields here are public because only |
        //| WebAccessor have privilage to use it so |
        //| it remain encapsulated.                 |
        //-------------------------------------------

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        private class NotificationDto
        {
            public long Id { get; private set; }
            public long UnixSelectedDate { get; private set; }
            public string ReminderText { get; private set; }
            public Int64 UserId { get; private set; }

            public NotificationDto(ReminderData data)
            {
                Id = data.Id;
                UnixSelectedDate = (long)data.SelectedDate.Subtract(UnixEpoch).TotalSeconds;
                ReminderText = data.ReminderText;
                UserId = data.UserId;
            }

            public ReminderData ToReminderData()
            {
                ReminderData data = new ReminderData()
                {
                    Id = Id,
                    SelectedDate = UnixEpoch.Add(TimeSpan.FromSeconds(UnixSelectedDate)),
                    ReminderText = ReminderText,
                    UserId = UserId
                };
                return data;
            }
        }

        private class UserDto
        {
            public Int64 Id { get; set; }
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Login { get; set; }
            public string PasswordHash { get; set; }
            public Int64 LastLogin { get; set; }
            public List<NotificationDto> Notifications { get; set; }
            public string Email { get; set; }

            public UserDto(UserData data)
            {
                Id = data.Id;
                Surname = data.Surname;
                Name = data.Name;
                Login = data.Login;
                PasswordHash = data.PasswordHash;
                LastLogin = (long)data.LastLogin.Subtract(UnixEpoch).TotalSeconds;
                Notifications = new List<NotificationDto>();
                foreach (var notification in data.Notifications)
                {
                    Notifications.Add(new NotificationDto(notification));
                }
                Email = data.Email;
            }

            public UserData ToUserData()
            {
                UserData data = new UserData()
                {
                    Id = Id,
                    Surname = Surname,
                    Name = Name,
                    Login = Login,
                    PasswordHash = PasswordHash,
                    LastLogin = UnixEpoch.Add(TimeSpan.FromSeconds(LastLogin)),
                    Notifications = new List<ReminderData>(),
                    Email = Email
                };
                foreach (var notification in Notifications)
                {
                    data.Notifications.Add(notification.ToReminderData());
                }
                return data;
            }
        }
    }
}
