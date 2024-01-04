using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Smart_Agenda_API.DTO;
using Smart_Agenda_DAL;
using Smart_Agenda_Logic.Domain;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTest
{
    [TestClass]
    public class TaskControllerIntegrationTest
    {

        private static WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private IConfigurationRoot _configuration;
        private DataBase _context;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, MockAuthentication>();
                });
            });
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<DataBase>();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            DatabaseSeeder.Seed(_context);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_ReturnsOk_WhenTaskIsValid()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());

            var taskCreationDTO = new TaskCreationDTO
            {
                TaskName = "Task1",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Low,
                Status = false,
                CalendarId = 5
            };

            var content = JsonContent.Create(taskCreationDTO);
            var url = "/task";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_ReturnsBadRequest_WhenTaskIsInvalid()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());
            var taskCreationDTO = new TaskCreationDTO
            {
                TaskName = "",
                DueDate = new DateTime(2020, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Low,
                Status = false,
                CalendarId = 5
            };

            var content = JsonContent.Create(taskCreationDTO);
            var url = "/task";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_ReturnsUnauthorized_WhenUserIsUnauthorized()
        {
            // Arrange
            var taskCreationDTO = new TaskCreationDTO
            {
                TaskName = "Task1",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Low,
                Status = false,
                CalendarId = 5
            };

            var content = JsonContent.Create(taskCreationDTO);
            var url = "/task";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_ReturnsOk_WhenTaskIsValid()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());
            var taskUpdateDTO = new TaskUpdateDTO
            {
                TaskName = "Task1",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Low,
                Status = false,
            };

            var content = JsonContent.Create(taskUpdateDTO);
            var url = "/task/5";

            // Act
            var response = await _client.PutAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<Smart_Agenda_Logic.Domain.Task>(responseContent);
            Assert.IsNotNull(task);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());
            var taskUpdateDTO = new TaskUpdateDTO
            {
                TaskName = "Task1",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Low,
                Status = false,
            };

            var content = JsonContent.Create(taskUpdateDTO);
            var url = "/task/0";

            // Act
            var response = await _client.PutAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task UpdateTask_ReturnsUnauthorized_WhenUserIsUnauthorized()
        {
            // Arrange
            var taskUpdateDTO = new TaskUpdateDTO
            {
                TaskName = "Task1",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Low,
                Status = false,
            };

            var content = JsonContent.Create(taskUpdateDTO);
            var url = "/task/5";

            // Act
            var response = await _client.PutAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task GetTask_ReturnsOk_WhenTaskExists()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());
            var url = "/task/5";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task GetTask_ReturnsNotFound_WhenTaskIsNotFound()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());
            var url = "/task/0";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task GetTask_ReturnsUnauthorized_WhenUserIsUnauthorized()
        {
            // Arrange
            var url = "/task/5";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task DeleteTask_ReturnsOk_WhenTaskIsDeleted()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());
            var url = "/task/5";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DeleteTask_ReturnsUnauthorized_WhenUserIsUnauthorized()
        {
            // Arrange
            var url = "/task/5";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DeleteTask_ReturnsNotFound_WhenTaskIsNotFound()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());
            var url = "/task/0";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}