using System;
using System.Data.Entity.ModelConfiguration;

namespace DataStorage.Models
{
    [Serializable]
    public class ReminderData : IComparable
    {
        // Private parts with data

        // Properties for private fields
        public long Id { get; private set; }

        public DateTime SelectedDate { get; set; }

        public string ReminderText { get; set; }

        public Int64 UserId { get; set; }

        public UserData User { get; set; }
        
        //This constructor will be used by EF
        public ReminderData()
        {
            SelectedDate = DateTime.Now;
        }

        // Make initialization safe using without NullReferenceException.
        public ReminderData(UserData user)
        {
            SelectedDate = DateTime.Now;
            User = user;
            UserId = user.Id;
            user.Notifications.Add(this);
        }

        public int CompareTo(object obj)
        {
            if (obj != null && obj is ReminderData)
            {
                ReminderData r = (ReminderData)obj;
                return (int)Math.Ceiling((SelectedDate - r.SelectedDate).TotalMinutes + 0.5);
            }
            throw new Exception();
        }

        public bool IsTimeCome()
        {
            return SelectedDate <= DateTime.Now;
        }
    }

    internal class ReminderEntityConfiguration : EntityTypeConfiguration<ReminderData>
    {
        public ReminderEntityConfiguration()
        {
            ToTable("Remindings");
            HasKey(r => r.Id);

            Property(r => r.Id)
                .HasColumnName("Id")
                .IsRequired();
            Property(r => r.ReminderText)
                .HasColumnName("NotificationText")
                .IsRequired();
            Property(r => r.SelectedDate)
                .HasColumnName("SelectedDate")
                .IsRequired();
            Property(r => r.UserId)
                .HasColumnName("UserId")
                .IsRequired();
        }
    }
}
