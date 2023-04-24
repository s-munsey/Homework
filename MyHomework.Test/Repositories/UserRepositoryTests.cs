using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using MyHomework.Config;
using MyHomework.Entities;
using MyHomework.Repositories;
using MyHomework.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyHomework.Test.Repositories
{
    public class UserRepositoryTests
    {
        private Mock<IFileService> _fileService;
        private Mock<ILogger<UserRepository>> _logger;

        public UserRepositoryTests()
        {
            _fileService = new Mock<IFileService>();
            _logger = new Mock<ILogger<UserRepository>>();
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsExpectedUsers()
        {
            // Arrange
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"results\": [{\"name\": {\"last\": \"Doe\", \"first\": \"John\"}, \"location\": {\"city\": \"New York\"}, \"email\": \"johndoe@example.com\", \"dob\": {\"age\": 30}}]}")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var appSettings = new AppSettings { Uri = "https://api.example.com" };
            var options = new Mock<IOptions<AppSettings>>();
            options.Setup(x => x.Value).Returns(appSettings);
            var userRepository = new UserRepository(httpClient, _fileService.Object, options.Object, _logger.Object);

            // Act
            var users = await userRepository.GetUsersAsync(1);

            // Assert
            Assert.NotNull(users);
            Assert.Single(users);
            Assert.Equal("Doe", users[0].LastName);
            Assert.Equal("John", users[0].FirstName);
            Assert.Equal("New York", users[0].City);
            Assert.Equal("johndoe@example.com", users[0].Email);
            Assert.Equal("30", users[0].Age);
        }
    }
}
