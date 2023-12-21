using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Smart_Agenda_Logic.Managers
{
    public class TaskManager
    {

        private readonly ITaskDAL _taskDAL;
        public TaskManager(ITaskDAL taskDAL)
        {
            _taskDAL = taskDAL;
        }

        private async Task<T> ExecuteUserOperationAsync<T>(Func<Task<T>> operation, Func<Exception, TaskException> exceptionTransformer)
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
        public async Task<Domain.Task> AddTask(Domain.Task task)
        {

            return await ExecuteUserOperationAsync(async () =>
            {
                if (task.DueDate < DateTime.Now)
                {
                    throw new AddTaskException("Task date can't be in the past.");
                }
                return await _taskDAL.AddTask(task);
            }, ex => new TaskException(ex.Message, ex));
        }

        public async Task<Domain.Task> GetTask(int id)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                return await _taskDAL.GetTask(id);
            }, ex => new TaskException(ex.Message, ex));
        }

        public async Task<Domain.Task> UpdateTask(Domain.Task task)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (task.DueDate < DateTime.Now)
                {
                    throw new UpdateTaskException("Task date can't be in the past.");
                }
                return await _taskDAL.UpdateTask(task);
            }, ex => new TaskException(ex.Message, ex));
        }

        public async Task<Domain.Task> DeleteTask(int id)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                return await _taskDAL.DeleteTask(id);
            }, ex => new TaskException(ex.Message, ex));
        }


    }
}
