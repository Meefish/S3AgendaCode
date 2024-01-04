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

        private void checkCalendarExists(int calendarId)
        {
            bool calendarExists = _context.Calendar
                                         .Any(calendar => calendar.CalendarId == calendarId);
            if (!calendarExists)
            {
                throw new CalendarException($"The calendar with ID {calendarId} does not exist.");
            }
        }



        public async Task<List<Smart_Agenda_Logic.Domain.Task>> GetAllCalendarTasks(int calendarId)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                checkCalendarExists(calendarId);

                var tasks = await _context.Task
                                          .Where(task => task.CalendarId == calendarId)
                                          .ToListAsync();

                return tasks;
            }, ex => new CalendarException("Retrieving tasks went wrong", ex));
        }

        public async Task DeleteAllCalendarTasks(int calendarId)
        {
            try
            {
                checkCalendarExists(calendarId);

                var tasks = await _context.Task
                                 .Where(task => task.CalendarId == calendarId)
                                 .ToListAsync();

                _context.Task.RemoveRange(tasks);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw new CalendarException("Deleting tasks went wrong", ex);
            }
        }

        public async Task<Smart_Agenda_Logic.Domain.Calendar> GetCalendarForUser(int userId)
        {
            return await ExecuteDbOperationAsync(async () =>
            {

                var calendar = await _context.Calendar
                                              .Where(calendar => calendar.UserId == userId)
                                              .FirstOrDefaultAsync();
                if (calendar == null)
                {
                    throw new CalendarException("No calendar found for user.");
                }

                return calendar;
            }, ex => new CalendarException(ex.Message, ex));
        }

        public async Task<List<Smart_Agenda_Logic.Domain.Task>> CheckTaskEvent(int calendarId)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var currentTime = DateTime.Now;
                //  var checkInterval = TimeSpan.FromSeconds(15);  // If you don't want to fry the database
                var checkInterval = TimeSpan.FromMilliseconds(35); // The 35 milliseconds are added because without the polling 
                var tasks = await _context.Task                    // the inbox would be spammed with notifications lol
                           .Where(task => task.CalendarId == calendarId &&
                            task.DueDate >= currentTime &&
                            task.DueDate <= currentTime + checkInterval)
                           .ToListAsync();

                return tasks;
            }, ex => new CalendarException("Checking task event went wrong", ex));
        }

    }
}
