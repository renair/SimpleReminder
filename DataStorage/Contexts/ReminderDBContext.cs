using System.Data.Entity;
using DataStorage.Migrations;
using DataStorage.Models;

namespace DataStorage.DataAccess
{
    class ReminderDBContext : DbContext
    {
        public ReminderDBContext() : base("ReminderDataBase")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReminderDBContext, Configuration>());
        }

        public DbSet<UserData> Users { get; set; }
        public DbSet<ReminderData> Remindings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new ReminderEntityConfiguration());
        }
    }
}
