using Smart_Agenda_API;
using Smart_Agenda_DAL;
using Smart_Agenda_Logic.Domain;

namespace IntegrationTest
{
    internal static class DatabaseSeeder
    {
        private static DataBase _context;

        public static void Seed(DataBase context)
        {
            _context = context;
            SeedUsers();
            SeedCalendars();
            SeedTasks();
        }

        private static void SeedUsers()
        {
            string passwordHash = PasswordHasher.HashPassword("TestPassword123!");
            _context.User.Add(new User
            {
                UserId = 5,
                Username = "TestUser",
                PasswordHash = passwordHash,
                Email = "testuser@example.com",
                UserRole = UserRole.User
            });
            _context.SaveChanges();
        }

        private static void SeedCalendars()
        {
            _context.Calendar.Add(new Calendar
            {
                CalendarId = 5,
                Preference = "Preference",
                UserId = 5
            });
            _context.SaveChanges();
        }

        private static void SeedTasks()
        {
            _context.Task.Add(new Smart_Agenda_Logic.Domain.Task
            {
                TaskId = 5,
                TaskName = "TestTask",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 5
            });

            _context.Task.Add(new Smart_Agenda_Logic.Domain.Task
            {
                TaskId = 6,
                TaskName = "TestTask2",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Medium,
                Status = false,
                CalendarId = 5
            });
            _context.SaveChanges();

        }

    }
}
