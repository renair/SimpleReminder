using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DataStorage.Models
{
    [Serializable]
    public class UserData
    {
        public Int64 Id { get; private set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public DateTime LastLogin { get; set; }

        public List<ReminderData> Notifications { get; set; }

        public string Email { get; set; }

        public UserData()
        {
            Notifications = new List<ReminderData>();
        }

        //internal UserData( UserData copySrc = null)
        //{
        //    if (copySrc == null) return;
        //    Surname = copySrc.Surname;
        //    Name = copySrc.Name;
        //    Login = copySrc.Login;
        //    PasswordHash = copySrc.PasswordHash;
        //    LastLogin = copySrc.LastLogin;
        //    Email = copySrc.Email;
        //    Notifications = copySrc.Notifications;
        //}
    }

    internal class UserEntityConfiguration : EntityTypeConfiguration<UserData>
    {
        public UserEntityConfiguration()
        {
            ToTable("Users");
            HasKey(u => u.Id);

            Property(u => u.Id)
                .HasColumnName("Id")
                .IsRequired();
            Property(u => u.Surname)
                .HasColumnName("Surname")
                .IsRequired();
            Property(u => u.Name)
                .HasColumnName("Name")
                .IsRequired();
            Property(u => u.Login)
                .HasColumnName("Login")
                .IsRequired();
            Property(u => u.PasswordHash)
                .HasColumnName("Password")
                .IsRequired();
            Property(u => u.LastLogin)
                .HasColumnName("LastLogin")
                .IsRequired();
            Property(u => u.Email)
                .HasColumnName("Email")
                .IsOptional();

            HasMany(u => u.Notifications)
                .WithRequired(n => n.User)
                .HasForeignKey(n => n.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}
