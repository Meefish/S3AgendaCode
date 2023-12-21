using Microsoft.EntityFrameworkCore;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Smart_Agenda_DAL
{
    public class CalendarDAL : ICalendarDAL
    {
        private readonly DataBase _context;
        public CalendarDAL(DataBase context)
        {
            _context = context;
        }

        private async Task<T> ExecuteDbOperationAsync<T>(Func<Task<T>> operation, Func<Exception, Exception> exceptionTransformer)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw exceptionTransformer(ex);
            }
        }

        public async Task<List<Smart_Agenda_Logic.Domain.Task>> RetrieveAllCalendarTasks(int calendarId)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var tasks = await _context.Task
                                          .Where(task => task.CalendarId == calendarId)
                                          .ToListAsync();
                return tasks;
            }, ex => new CalendarException("Retrieving tasks went wrong", ex));
        }

        public async Task DeleteAllCalendarTasks(int calendarId)
        {
            var tasks = await _context.Task
                             .Where(task => task.CalendarId == calendarId)
                             .ToListAsync();

            _context.Task.RemoveRange(tasks);
            await _context.SaveChangesAsync();
        }

        public async Task<Smart_Agenda_Logic.Domain.Calendar> GetCalendarForUser(int userId)
        {
            return await _context.Calendar
                                 .Where(calendar => calendar.UserId == userId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<Smart_Agenda_Logic.Domain.Task>> CheckTaskEvent(int calendarId)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var currentTime = DateTime.Now;
                var checkInterval = TimeSpan.FromMinutes(2);
                var tasks = await _context.Task
                           .Where(task => task.CalendarId == calendarId &&
                                            task.DueDate >= currentTime &&
                                            task.DueDate <= currentTime + checkInterval)
                           .ToListAsync();

                return tasks;
            }, ex => new CalendarException("Checking task event went wrong", ex));
        }

    }
}
