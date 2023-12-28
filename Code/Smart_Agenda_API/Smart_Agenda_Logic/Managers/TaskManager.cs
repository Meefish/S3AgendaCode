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
                if (task.DueDate < DateTime.Now - TimeSpan.FromMinutes(5))
                {
                    if (task.DueDate == DateTime.MinValue)
                    {
                        throw new AddTaskException("Task date can't be empty.");//This will also cover the check for if task.DueDate == null, you can't really null check
                    }                                                           //DateTimes, they are always initialized to 1/1/0001 00:00:00 if not set
                    throw new AddTaskException("Task date can't be in the past.");
                }
                if (string.IsNullOrEmpty(task.TaskName))
                {
                    throw new AddTaskException("Task name can't be empty.");
                }

                if (task.TaskName.Length > 50)
                {
                    throw new AddTaskException("Task name can't exceed 50 characters.");
                }

                if (task.CalendarId <= 0)
                {
                    throw new AddTaskException("The calendar doesn't exist.");
                }

                return await _taskDAL.AddTask(task);
            }, ex => new TaskException(ex.Message, ex));
        }

        public async Task<Domain.Task> GetTask(int id)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (id <= 0)
                {
                    throw new RetrieveTaskException("Task id can't be empty.");
                }
                return await _taskDAL.GetTask(id);
            }, ex => new TaskException(ex.Message, ex));
        }

        public async Task<Domain.Task> UpdateTask(Domain.Task task)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (task.DueDate < DateTime.Now - TimeSpan.FromMinutes(5))
                {
                    if (task.DueDate == DateTime.MinValue)
                    {
                        throw new UpdateTaskException("Task date can't be empty.");
                    }
                    throw new UpdateTaskException("Task date can't be in the past.");
                }

                if (string.IsNullOrEmpty(task.TaskName))
                {
                    throw new UpdateTaskException("Task name can't be empty.");
                }

                if (task.TaskName.Length > 50)
                {
                    throw new UpdateTaskException("Task name can't exceed 50 characters.");
                }

                if (task.CalendarId <= 0)
                {
                    throw new UpdateTaskException("The calendar doesn't exist.");
                }

                if (task.TaskId <= 0)
                {
                    throw new UpdateTaskException("Task does not exist.");
                }

                return await _taskDAL.UpdateTask(task);
            }, ex => new TaskException(ex.Message, ex));
        }

        public async Task<Domain.Task> DeleteTask(int id)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (id <= 0)
                {
                    throw new RetrieveTaskException("Task id can't be empty.");
                }
                return await _taskDAL.DeleteTask(id);
            }, ex => new TaskException(ex.Message, ex));
        }


    }
}
