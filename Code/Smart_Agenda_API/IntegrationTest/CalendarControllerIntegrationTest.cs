using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smart_Agenda_DAL;
using Smart_Agenda_Logic.Domain;
using System.Net;
using System.Net.Http.Headers;

namespace IntegrationTest
{
    [TestClass]
    public class CalendarControllerIntegrationTest
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
        public async System.Threading.Tasks.Task GetAllCalendarTasks_ReturnsOk_WhenCalendarTaskIsValid()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());

            var url = "/calendar/5";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task GetAllCalendarTasks_ReturnsUnauthorized_WhenUserIsUnauthorized()
        {
            // Arrange
            var url = "/calendar/5";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [TestMethod]
        public async System.Threading.Tasks.Task GetAllCalendarTasks_ReturnsNotFound_WhenCalendarIsNotFound()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());

            var url = "/calendar/999";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DeleteAllCalendarTasks_ReturnsOk_WhenCalendarTasksAreDeleted()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.Admin.ToString());

            var url = "/calendar/5";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DeleteAllCalendarTasks_ReturnsUnauthorized_WhenUserIsUnauthorized()
        {
            // Arrange
            var url = "/calendar/5";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DeleteAllCalendarTasks_ReturnsForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());

            var url = "/calendar/5";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task DeleteAllCalendarTasks_ReturnsNotFound_WhenCalendarIsNotFound()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.Admin.ToString());

            var url = "/calendar/999";

            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }



    }
}
