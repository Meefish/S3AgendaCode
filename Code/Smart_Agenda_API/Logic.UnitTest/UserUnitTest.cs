using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;
using Smart_Agenda_Logic.Managers;
using Task = System.Threading.Tasks.Task;

namespace Logic.UnitTest
{
    [TestClass]
    public class UserUnitTest
    {

        private IUserDAL _mockUserDAL;

        [TestInitialize]
        public void Setup()
        {
            _mockUserDAL = new MockUserDAL();
        }

        [TestMethod]
        public async Task AddUser_UserMustContainName()
        {

            // Arrange
            var newUser = new User
            {
                Username = "",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = "jan99@example.com"
            };
            UserManager userManager = new UserManager(_mockUserDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                                async () => await userManager.AddUser(newUser));
            Assert.AreEqual("Username can't be empty.", exception.Message);
        }

        [TestMethod]
        public async Task AddUser_UsernameCannotExceed12Character()
        {

            // Arrange
            var newUser = new User
            {
                Username = "JanTheAmazing",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = "jan99@example.com"
            };
            UserManager userManager = new UserManager(_mockUserDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                                async () => await userManager.AddUser(newUser));
            Assert.AreEqual("Username can't exceed 12 characters.", exception.Message);
        }

        [TestMethod]
        public async Task AddUser_UserMustContainEmail()
        {

            // Arrange
            var newUser = new User
            {
                Username = "Jan99",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = ""
            };
            UserManager userManager = new UserManager(_mockUserDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                                async () => await userManager.AddUser(newUser));
            Assert.AreEqual("Email can't be empty.", exception.Message);
        }


        [TestMethod]
        public async Task GetUser_MustContainUserId()
        {
            //  Arrange
            int userId = 0;
            UserManager userManager = new UserManager(_mockUserDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                              async () => await userManager.GetUser(userId));
            Assert.AreEqual("The user doesn't exist.", exception.Message);
        }

        [TestMethod]
        public async Task GetUserByEmail_MustContainEmail()
        {
            //  Arrange
            string email = "";
            UserManager userManager = new UserManager(_mockUserDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                              async () => await userManager.GetUserByEmail(email));
            Assert.AreEqual("The user doesn't exist.", exception.Message);
        }

        [TestMethod]
        public async Task UpdateUser_UserMustContainUsername()
        {

            // Arrange
            var updateUser = new User
            {
                Username = "",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = "jan99@example.com",
                UserId = 1

            };
            UserManager taskManager = new UserManager(_mockUserDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                         async () => await taskManager.UpdateUser(updateUser));
            Assert.AreEqual("Username can't be empty.", exception.Message);
        }

        [TestMethod]
        public async Task UpdateUser_UsernameCannotExceed12Character()
        {

            // Arrange
            var updateUser = new User
            {
                Username = "JanTheAmazing",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = "jan99@example.com",
                UserId = 1
            };
            UserManager taskManager = new UserManager(_mockUserDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                         async () => await taskManager.UpdateUser(updateUser));
            Assert.AreEqual("Username can't exceed 12 characters.", exception.Message);

        }

        [TestMethod]
        public async Task UpdateUser_MustContainEmail()
        {

            // Arrange
            var updateUser = new User
            {
                Username = "Jan",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = "",
                UserId = 1
            };
            UserManager taskManager = new UserManager(_mockUserDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                         async () => await taskManager.UpdateUser(updateUser));
            Assert.AreEqual("Email can't be empty.", exception.Message);
        }

        [TestMethod]
        public async Task UpdateUser_MustContainUserId()
        {

            // Arrange
            var updateUser = new User
            {
                Username = "Jan",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = "jan99@example.com",
                UserId = 0
            };
            UserManager taskManager = new UserManager(_mockUserDAL);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                         async () => await taskManager.UpdateUser(updateUser));
            Assert.AreEqual("The user doesn't exist.", exception.Message);
        }

        [TestMethod]
        public async Task DeleteTask_MustContainTaskId()
        {
            //  Arrange
            int userId = 0;
            UserManager userManager = new UserManager(_mockUserDAL);


            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<UserException>(
                              async () => await userManager.DeleteUser(userId));
            Assert.AreEqual("The user doesn't exist.", exception.Message);
        }
    }
}