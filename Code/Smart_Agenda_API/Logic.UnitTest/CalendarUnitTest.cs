using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;
using Smart_Agenda_Logic.Managers;

namespace Logic.UnitTest
{
    [TestClass]
    public class CalendarUnitTest
    {
        private ICalendarDAL _mockCalendarDAL;

        [TestInitialize]
        public void Setup()
        {
            _mockCalendarDAL = new MockCalendarDAL();
        }

        [TestMethod]
        public async Task GetAllCalendarTasks_MustContainCalendarId()
        {
            //  Arrange
            int calendarId = 0;
            CalendarManager calendarManager = new CalendarManager(_mockCalendarDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<CalendarException>(
                              async () => await calendarManager.GetAllCalendarTasks(calendarId));
            Assert.AreEqual("The calendar doesn't exist.", exception.Message);
        }

        [TestMethod]
        public async Task GetCalendarForUser_MustContainUserId()
        {
            //  Arrange
            int userId = 0;
            CalendarManager calendarManager = new CalendarManager(_mockCalendarDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<CalendarException>(
                              async () => await calendarManager.GetCalendarForUser(userId));
            Assert.AreEqual("The user doesn't exist.", exception.Message);
        }

        [TestMethod]
        public async Task DeleteAllCalendarTasks_MustContainCalendarId()
        {
            //  Arrange
            int calendarId = 0;
            CalendarManager calendarManager = new CalendarManager(_mockCalendarDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<CalendarException>(
                              async () => await calendarManager.DeleteAllCalendarTasks(calendarId));
            Assert.AreEqual("The calendar doesn't exist.", exception.Message);
        }


    }
}
