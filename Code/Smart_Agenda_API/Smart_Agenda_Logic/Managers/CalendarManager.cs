using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Smart_Agenda_Logic.Managers
{
    public class CalendarManager
    {

        private readonly ICalendarDAL _calendarDAL;
        public CalendarManager(ICalendarDAL calendarDAL)
        {
            _calendarDAL = calendarDAL;
        }

        private async Task<T> ExecuteUserOperationAsync<T>(Func<Task<T>> operation, Func<Exception, CalendarException> exceptionTransformer)
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

        public async Task<List<Domain.Task>> GetAllCalendarTasks(int calendarId)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (calendarId <= 0)
                {
                    throw new CalendarException("The calendar doesn't exist.");
                }
                return await _calendarDAL.GetAllCalendarTasks(calendarId);
            }, ex => new CalendarException(ex.Message, ex));
        }

        public async Task<Domain.Calendar> GetCalendarForUser(int userId)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (userId <= 0)
                {
                    throw new CalendarException("The user doesn't exist.");
                }
                return await _calendarDAL.GetCalendarForUser(userId);
            }, ex => new CalendarException(ex.Message, ex));
        }

        public async Task DeleteAllCalendarTasks(int calendarId)
        {
            try
            {
                if (calendarId <= 0)
                {
                    throw new CalendarException("The calendar doesn't exist.");
                }
                await _calendarDAL.DeleteAllCalendarTasks(calendarId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw new CalendarException(ex.Message, ex);
            }

        }


        /*
        public async Task<List<Domain.Task>> CheckTaskEvent(int calendarId)               //Didn't need to be checked in Manager, for getting the websocket learning goal approved
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                return await _calendarDAL.CheckTaskEvent(calendarId);
            }, ex => new CalendarException("Checking task event failed", ex));
        }
        */
    }
}
