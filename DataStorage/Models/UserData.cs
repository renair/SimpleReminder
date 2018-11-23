using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Security.Cryptography;
using System.Text;

namespace DataStorage.Models
{
    [Serializable]
    public class UserData
    {
        private string _password;

        public Int64 Id { get; internal set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; internal set; }

        public string Password
        {
            set
            {
                _password = value;
                PasswordHash = GetHash(value);
            }
        }

        public DateTime LastLogin { get; set; }

        public List<ReminderData> Notifications { get; set; }

        public string Email { get; set; }

        public UserData()
        {
            Notifications = new List<ReminderData>();
        }

        private string GetHash(string s)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }
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
