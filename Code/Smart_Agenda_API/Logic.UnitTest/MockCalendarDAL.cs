using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Logic.UnitTest
{
    public class MockCalendarDAL : ICalendarDAL
    {
        private readonly List<Calendar> _database = new List<Calendar>();

        public MockCalendarDAL()
        {
            var mockCalendar = new Calendar
            {
                CalendarId = 1,
                Preference = "Test",
                UserId = 1,

                Tasks = new List<Smart_Agenda_Logic.Domain.Task>()
              {
                  new Smart_Agenda_Logic.Domain.Task
                  {
                      TaskId = 1,
                      TaskName = "Task 1",
                      DueDate = DateTime.Now,
                      TaskPriority = TaskPriority.Low,
                      Status = false,
                      CalendarId = 1
                  },

                    new Smart_Agenda_Logic.Domain.Task
                  {
                      TaskId = 2,
                      TaskName = "Task 2",
                      DueDate = DateTime.Now,
                      TaskPriority = TaskPriority.Low,
                      Status = false,
                      CalendarId = 1
                  }

              }
            };
            _database.Add(mockCalendar);
        }


        public System.Threading.Tasks.Task DeleteAllCalendarTasks(int calendarId)
        {
            var calendar = _database.FirstOrDefault(c => c.CalendarId == calendarId);
            if (calendar == null)
            {
                throw new RetrieveTaskException("Calendar not found");
            }

            calendar.Tasks.Clear();
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public Task<Calendar> GetCalendarForUser(int userId)
        {
            var calendar = _database.FirstOrDefault(c => c.UserId == userId);
            if (calendar == null)
            {
                throw new RetrieveTaskException("Calendar not found");
            }
            return System.Threading.Tasks.Task.FromResult(calendar);
        }

        public Task<List<Smart_Agenda_Logic.Domain.Task>> GetAllCalendarTasks(int calendarId)
        {
            var calendar = _database.FirstOrDefault(c => c.CalendarId == calendarId);
            if (calendar == null)
            {
                throw new RetrieveTaskException("Calendar not found");
            }
            List<Smart_Agenda_Logic.Domain.Task> calendarTasks = calendar.Tasks as List<Smart_Agenda_Logic.Domain.Task> ?? calendar.Tasks.ToList();
            return System.Threading.Tasks.Task.FromResult(calendarTasks);
        }
        public Task<List<Smart_Agenda_Logic.Domain.Task>> CheckTaskEvent(int calendarId)
        {
            throw new NotImplementedException();
        }
    }
}
