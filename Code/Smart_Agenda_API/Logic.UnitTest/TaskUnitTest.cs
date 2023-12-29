using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;
using Smart_Agenda_Logic.Managers;

namespace Logic.UnitTest
{
    [TestClass]
    public class TaskUnitTest
    {

        private ITaskDAL? _mockTaskDAL;

        [TestInitialize]
        public void Setup()
        {
            _mockTaskDAL = new MockTaskDAL();
        }



        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_ShouldAddTaskToCalendar()  //Is this testing the mock or is this a way of testing if everything works right?
        {
            //Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Grocery shopping",
                DueDate = new DateTime(2024, 1, 27, 15, 0, 0),
                TaskPriority = TaskPriority.Medium,
                Status = false,
                CalendarId = 1
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);
            //Act
            var result = await taskManager.AddTask(newTask);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Grocery shopping", result.TaskName);
            Assert.AreEqual(new DateTime(2024, 1, 27, 15, 0, 0), result.DueDate);
            Assert.AreEqual(TaskPriority.Medium, result.TaskPriority);
            Assert.IsFalse(result.Status);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_TaskMustContainDate()
        {

            // Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Christmas Prep",
                DueDate = new DateTime(),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                                async () => await taskManager.AddTask(newTask));
            Assert.AreEqual("Task date can't be empty.", exception.Message);
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
                Status = false,
                CalendarId = 1

            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                                async () => await taskManager.AddTask(newTask));
            Assert.AreEqual("Task date can't be in the past.", exception.Message);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_TaskMustContainName()
        {

            // Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "",
                DueDate = new DateTime(2027, 2, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1

            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                                async () => await taskManager.AddTask(newTask));
            Assert.AreEqual("Task name can't be empty.", exception.Message);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_TaskNameCannotExceed50Characters()
        {

            // Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                DueDate = new DateTime(2027, 2, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                                async () => await taskManager.AddTask(newTask));
            Assert.AreEqual("Task name can't exceed 50 characters.", exception.Message);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_MustContainCalendarId()
        {

            // Arrange
            var newTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Buy a car.",
                DueDate = new DateTime(2027, 2, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 0
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                                async () => await taskManager.AddTask(newTask));
            Assert.AreEqual("The calendar doesn't exist.", exception.Message);
        }


        [TestMethod]
        public async System.Threading.Tasks.Task GetTask_MustContainTaskId()
        {
            //  Arrange
            int taskId = 0;
            TaskManager taskManager = new TaskManager(_mockTaskDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                              async () => await taskManager.GetTask(taskId));
            Assert.AreEqual("Task doesn't exist.", exception.Message);
        }


        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_ShouldUpdateTaskName()   //Is this testing the mock or is this a way of testing if everything works right?
        {

            //Arrange
            var updateTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Visiting the mall",
                DueDate = new DateTime(2024, 1, 27, 15, 0, 0),
                TaskPriority = TaskPriority.Medium,
                Status = false,
                CalendarId = 1,
                TaskId = 1

            };

            TaskManager taskManager = new TaskManager(_mockTaskDAL);
            //Act

            var updatedTask = await taskManager.UpdateTask(updateTask);

            //Assert
            Assert.AreEqual("Visiting the mall", updatedTask.TaskName);
        }


        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_TaskMustContainDate()
        {

            // Arrange
            var updatedTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Buy a car",
                DueDate = new DateTime(),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1,
                TaskId = 1
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                         async () => await taskManager.UpdateTask(updatedTask));
            Assert.AreEqual("Task date can't be empty.", exception.Message);
        }


        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_TaskCannotBeInThePast()
        {

            // Arrange
            var updatedTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Buy a car",
                DueDate = new DateTime(1961, 12, 25, 16, 30, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1,
                TaskId = 1
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                         async () => await taskManager.UpdateTask(updatedTask));
            Assert.AreEqual("Task date can't be in the past.", exception.Message);
        }


        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_MustContainName()
        {

            // Arrange
            var updatedTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "",
                DueDate = new DateTime(2027, 2, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1,
                TaskId = 1
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                         async () => await taskManager.UpdateTask(updatedTask));
            Assert.AreEqual("Task name can't be empty.", exception.Message);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_TaskNameCannotHaveMoreThan50Characters()
        {

            // Arrange
            var updatedTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                DueDate = new DateTime(2027, 2, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1,
                TaskId = 1
            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                         async () => await taskManager.UpdateTask(updatedTask));
            Assert.AreEqual("Task name can't exceed 50 characters.", exception.Message);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_MustContainCalendarId()
        {

            // Arrange
            var updatedTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Buy a car",
                DueDate = new DateTime(2027, 2, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 0,
                TaskId = 1

            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                         async () => await taskManager.UpdateTask(updatedTask));
            Assert.AreEqual("The calendar doesn't exist.", exception.Message);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_MustContainTaskId()
        {

            // Arrange
            var updatedTask = new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = "Buy a car",
                DueDate = new DateTime(2027, 2, 27, 15, 0, 0),
                TaskPriority = TaskPriority.High,
                Status = false,
                CalendarId = 1,

            };
            TaskManager taskManager = new TaskManager(_mockTaskDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                         async () => await taskManager.UpdateTask(updatedTask));
            Assert.AreEqual("Task doesn't exist.", exception.Message);
        }


        [TestMethod]
        public async System.Threading.Tasks.Task DeleteTask_MustContainTaskId()
        {
            //  Arrange
            int taskId = 0;
            TaskManager taskManager = new TaskManager(_mockTaskDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<TaskException>(
                              async () => await taskManager.DeleteTask(taskId));
            Assert.AreEqual("Task doesn't exist.", exception.Message);
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


