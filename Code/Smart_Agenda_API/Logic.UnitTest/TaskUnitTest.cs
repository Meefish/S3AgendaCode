using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;
using Smart_Agenda_Logic.Managers;

namespace Logic.UnitTest
{
    [TestClass]
    public class TaskUnitTest
    {

        private ITaskDAL _mockTaskDAL;

        [TestInitialize]
        public void Setup()
        {
            _mockTaskDAL = new MockTaskDAL();
        }

        [TestMethod]
        public void AddTask_ShouldAddTaskToCalendar()
        {
            //Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Grocery shopping",
                DueDate = new DateTime(2024, 1, 27, 15, 0, 0),
                TaskPriority = TaskPriority.Medium,
                Status = false
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);
            //Act
            var result = taskManager.AddTask(newTask).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Grocery shopping", result.TaskName);
            Assert.AreEqual(new DateTime(2024, 1, 27, 15, 0, 0), result.DueDate);
            Assert.AreEqual(TaskPriority.Medium, result.TaskPriority);
            Assert.IsFalse(result.Status);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_TaskCannotBeInThePast()
        {

            // Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Christmas Prep",
                DueDate = new DateTime(1961, 12, 25, 16, 30, 0),
                TaskPriority = TaskPriority.High,
                Status = false
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
          async () => await taskManager.AddTask(newTask)
      );
            Assert.AreEqual("Task date can't be in the past.", exception.Message);
        }
        [TestMethod]
        public void UpdateTask_ShouldUpdateTaskName()
        {

            //Arrange
            var originalTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Grocery shopping",
                DueDate = new DateTime(2024, 1, 27, 15, 0, 0),
                TaskPriority = TaskPriority.Medium,
                Status = false
            };

            TaskManager taskManager = new TaskManager(_mockTaskDAL);
            var addedTask = taskManager.AddTask(originalTask).Result;
            //Act
            addedTask.TaskName = "Visiting the mall";
            var updatedTask = taskManager.UpdateTask(addedTask).Result;

            //Assert
            Assert.AreEqual("Visiting the mall", updatedTask.TaskName);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_TaskCannotBeInThePast()
        {

            // Arrange
            var originalTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Christmas Prep",
                DueDate = new DateTime(2024, 1, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);
            var addedTask = taskManager.AddTask(originalTask).Result;
            addedTask.DueDate = new DateTime(1961, 12, 25, 16, 30, 0);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                         async () => await taskManager.UpdateTask(addedTask));
            Assert.AreEqual("Task date can't be in the past.", exception.Message);
        }
    }
}
/*
        [TestMethod]
        public void AddTask_WithoutNameToCalendar()
        {
            //Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "",
                DueDate = new DateTime(2024, 1, 31, 15, 0, 0),
                TaskPriority = TaskPriority.Urgent,
                Status = false
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);
            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => taskManager.AddTask(newTask).Result);


            //Moeten taken van de Database(NullExceptions) die ik eerst als exception in de code had staan, hier ook getest worden?
        }
*/