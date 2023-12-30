using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Smart_Agenda_API.DTO;
using Smart_Agenda_Logic.Domain;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTest
{
    [TestClass]
    public class TaskControllerIntegrationTest
    {

        private static WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
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
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("TestScheme");
        }


        [TestMethod]
        public async System.Threading.Tasks.Task AddTask_ReturnsOk_WhenTaskIsValid()
        {
            // Arrange
            var taskCreationDTO = new TaskCreationDTO
            {
                TaskName = "Task1",
                DueDate = new DateTime(2030, 4, 2, 0, 0, 0),
                TaskPriority = TaskPriority.Low,
                Status = false,
                CalendarId = 1
            };

            var content = JsonContent.Create(taskCreationDTO);
            var url = "/task";

            // Act
            var response = await _client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<Smart_Agenda_Logic.Domain.Task>(responseContent);
            Assert.IsNotNull(task);
        }
    }
}