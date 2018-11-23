using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using DataStorage.Models;
using Tools;

namespace DataStorage.DataAccess
{
    public class WebAccessor : IDataAccess
    {
        //// Method to get has as string from string.
        //// Writed for hashing passwords.
        //private string GetHash(string s)
        //{
        //    HashAlgorithm algorithm = SHA256.Create();
        //    byte[] bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
        //    StringBuilder builder = new StringBuilder();
        //    foreach(byte b in bytes)
        //    {
        //        builder.Append(b.ToString("X2"));
        //    }
        //    return builder.ToString();
        //}

        private string _baseUri;
        private HttpClient _client;

        public WebAccessor(string baseURI)
        { 
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseURI);
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
                return response.Content.ReadAsAsync<UserData>().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Logger.Log("Can't sent authorize request", e);
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
                List<NotificationDTO> notifications = response.Content.ReadAsAsync<List<NotificationDTO>>().GetAwaiter().GetResult();
                List<ReminderData> result = new List<ReminderData>();
                foreach (NotificationDTO notification in notifications)
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

        public UserData SignUp(UserData userData, string password)
        {
            try
            {
                HttpResponseMessage response = _client.PostAsJsonAsync("register", userData).GetAwaiter().GetResult();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Log("Call 'register' endpoint unsuccesfull");
                    return null;
                }
                return response.Content.ReadAsAsync<UserData>().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Logger.Log("Can't sent register request", e);
            }
            return null;
        }

        public bool AddNotification(ReminderData data)
        {
            try
            {
                NotificationDTO dto = new NotificationDTO(data);
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
                NotificationDTO dto = new NotificationDTO(data);
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
                NotificationDTO dto = new NotificationDTO(data);
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

        private class NotificationDTO
        {
            public long Id { get; set; }
            public long UnixSelectedDate { get; set; }
            public string ReminderText { get; set; }
            public Int64 UserId { get; set; }

            private static DateTime UnixEpoch = new DateTime(1970, 1, 1);

            public NotificationDTO(ReminderData data)
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
    }
}
