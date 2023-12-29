namespace Smart_Agenda_Logic.Interfaces
{
    public interface ICalendarDAL
    {

        Task<List<Domain.Task>> GetAllCalendarTasks(int calendarId);
        Task DeleteAllCalendarTasks(int calendarId);

        Task<Domain.Calendar> GetCalendarForUser(int userId);

        Task<List<Domain.Task>> CheckTaskEvent(int calendarId);
    }
}
