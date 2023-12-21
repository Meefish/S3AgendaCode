using Microsoft.EntityFrameworkCore;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Smart_Agenda_DAL
{
    public class TaskDAL : ITaskDAL
    {
        private readonly DataBase _context;

        public TaskDAL(DataBase context)
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

        public async Task<Smart_Agenda_Logic.Domain.Task> AddTask(Smart_Agenda_Logic.Domain.Task task)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                _context.Task.Add(task);
                await _context.SaveChangesAsync();
                return task;
            }, ex => new AddTaskException("Adding a new task went wrong", ex));
        }

        public async Task<Smart_Agenda_Logic.Domain.Task> GetTask(int id)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var task = await _context.Task.FindAsync(id);
                if (task == null)
                {
                    throw new RetrieveTaskException("Task not found");
                }
                return task;
            }, ex => new RetrieveTaskException("Retrieving a task went wrong", ex));
        }

        public async Task<Smart_Agenda_Logic.Domain.Task> UpdateTask(Smart_Agenda_Logic.Domain.Task task)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return task;
            }, ex => new UpdateTaskException("Updating a task went wrong", ex));
        }

        public async Task<Smart_Agenda_Logic.Domain.Task> DeleteTask(int id)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var task = await _context.Task.FindAsync(id);
                if (task == null)
                {
                    throw new DeleteTaskException("Task not found");
                }
                _context.Task.Remove(task);
                await _context.SaveChangesAsync();
                return task;
            }, ex => new DeleteTaskException("Deleting a task went wrong", ex));
        }


    }
}
