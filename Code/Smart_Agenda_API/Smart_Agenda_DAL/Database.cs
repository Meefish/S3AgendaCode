using Microsoft.EntityFrameworkCore;
using Smart_Agenda_Logic.Domain;

namespace Smart_Agenda_DAL
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options) : base(options)
        {
        }
        public DbSet<Smart_Agenda_Logic.Domain.Task> Task { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Calendar> Calendar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Smart_Agenda_Logic.Domain.Task>()
          .HasOne(t => t.Calendar)
          .WithMany(c => c.Tasks)
          .HasForeignKey(t => t.CalendarId);

            modelBuilder.Entity<User>()
          .HasOne(u => u.Calendar)
          .WithOne(c => c.User)
          .HasForeignKey<Calendar>(c => c.UserId);

        }


    }
}
