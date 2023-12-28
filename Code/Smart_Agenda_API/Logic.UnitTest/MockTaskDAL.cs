using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Logic.UnitTest
{
    public class MockTaskDAL : ITaskDAL
    {
        private readonly List<Smart_Agenda_Logic.Domain.Task> _calendar = new List<Smart_Agenda_Logic.Domain.Task>();
        private int _idCounter = 1;

        public MockTaskDAL()
        {
            var mockTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskId = _idCounter++,
                TaskName = "Task 1",
                DueDate = DateTime.Now,
                TaskPriority = TaskPriority.Low,
                Status = false,
                CalendarId = 1
            };
            _calendar.Add(mockTask);
        }

        public Task<Smart_Agenda_Logic.Domain.Task> AddTask(Smart_Agenda_Logic.Domain.Task task)
        {

            task.TaskId = _idCounter++;
            _calendar.Add(task);


            return System.Threading.Tasks.Task.FromResult(task);
        }

        public Task<Smart_Agenda_Logic.Domain.Task> GetTask(int id)
        {
            var task = _calendar.FirstOrDefault(t => t.TaskId == id);
            if (task == null)
            {
                throw new RetrieveTaskException("Task not found");
            }
            return System.Threading.Tasks.Task.FromResult(task);
        }

        public Task<Smart_Agenda_Logic.Domain.Task> UpdateTask(Smart_Agenda_Logic.Domain.Task task)
        {

            var existingTask = _calendar.FirstOrDefault(t => t.TaskId == task.TaskId);
            if (existingTask == null)
            {
                throw new UpdateTaskException("Task not found");
            }

            existingTask.TaskName = task.TaskName;
            existingTask.DueDate = task.DueDate;
            existingTask.TaskPriority = task.TaskPriority;
            existingTask.Status = task.Status;
            existingTask.CalendarId = task.CalendarId;


            return System.Threading.Tasks.Task.FromResult(existingTask);
        }

        public Task<Smart_Agenda_Logic.Domain.Task> DeleteTask(int id)
        {

            var task = _calendar.FirstOrDefault(t => t.TaskId == id);
            if (task == null)
            {
                throw new DeleteTaskException("Task not found");
            }
            _calendar.Remove(task);


            return System.Threading.Tasks.Task.FromResult(task);
        }
    }
}
