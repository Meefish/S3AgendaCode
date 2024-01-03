using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smart_Agenda_API.DTO;
using Smart_Agenda_DAL;
using Smart_Agenda_Logic.Domain;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTest
{
    [TestClass]
    public class UserControllerIntegrationTest
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
        public async System.Threading.Tasks.Task AddUser_ReturnsOk_WhenUserIsValid()
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.Admin.ToString());

            var userCreationDTO = new UserCreationDTO
            {
                Name = "TestUser",
                Password = "TestPassword123!",
                Email = "testuser123@example.com"
            };

            var content = JsonContent.Create(userCreationDTO);
            var url = "/user/register";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddUser_ReturnsBadRequest_WhenUserIsInvalid()
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.Admin.ToString());

            var userCreationDTO = new UserCreationDTO
            {
                Name = "",
                Password = "TestPassword123!",
                Email = "testuser123@example.com"
            };

            var content = JsonContent.Create(userCreationDTO);
            var url = "/user/register";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddUser_ReturnsForbidden_WhenUserIsNotAdmin()
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(UserRole.User.ToString());

            var userCreationDTO = new UserCreationDTO
            {
                Name = "TestUser",
                Password = "TestPassword123!",
                Email = "testuser123@example.com"
            };

            var content = JsonContent.Create(userCreationDTO);
            var url = "/user/register";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task AddUser_ReturnsUnauthorized_WhenUserIsUnauthorized()
        {
            //Arrange
            var userCreationDTO = new UserCreationDTO
            {
                Name = "TestUser",
                Password = "TestPassword123!",
                Email = "testuser123@example.com"
            };

            var content = JsonContent.Create(userCreationDTO);
            var url = "/user/register";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task Login_ReturnsOk_WhenUserIsValid()
        {
            //Arrange
            var loginDTO = new LoginDTO
            {
                Email = "testuser@example.com",
                Password = "TestPassword123!"
            };

            var content = JsonContent.Create(loginDTO);
            var url = "/user/login";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task Login_ReturnsBadRequest_WhenUserIsInvalid()
        {
            //Arrange
            var loginDTO = new LoginDTO
            {
                Email = "",
                Password = "TestPassword123!"
            };

            var content = JsonContent.Create(loginDTO);
            var url = "/user/login";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public async System.Threading.Tasks.Task Login_ReturnsUnauthorized_WhenUserPasswordIsIncorrect()
        {
            //Arrange
            var loginDTO = new LoginDTO
            {
                Email = "testuser@example.com",
                Password = "TestPassword"
            };

            var content = JsonContent.Create(loginDTO);
            var url = "/user/login";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task Login_ReturnsNotFound_WhenUserEmailIsNotFound()
        {
            //Arrange
            var loginDTO = new LoginDTO
            {
                Email = "testuser123123@example.com",
                Password = "TestPassword123"
            };

            var content = JsonContent.Create(loginDTO);
            var url = "/user/login";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }



    }
}
