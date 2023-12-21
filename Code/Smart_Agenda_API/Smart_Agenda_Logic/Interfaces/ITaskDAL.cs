namespace Smart_Agenda_Logic.Interfaces
{
    public interface ITaskDAL
    {
        Task<Domain.Task> AddTask(Domain.Task task);
        Task<Domain.Task> GetTask(int id);
        Task<Domain.Task> UpdateTask(Domain.Task task);
        Task<Domain.Task> DeleteTask(int id);
    }
}
