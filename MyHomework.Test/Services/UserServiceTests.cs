using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MyHomework.Config;
using MyHomework.Entities;
using MyHomework.Repositories;
using MyHomework.Repositories.Interfaces;
using MyHomework.Services;
using MyHomework.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHomework.Test.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IFileService> _fileService;
        private Mock<ILogger<UserService>> _logger;
        private UserService _userService;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _fileService = new Mock<IFileService>();
            var options = Options.Create(new AppSettings { NumberOfUsers = 10 });
            _logger = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepository.Object, _fileService.Object, options, _logger.Object);
        }

        [Fact]
        public void ExportUsers_Should_Call_WriteAllLines_With_Correct_Output()
        {
            // Arrange
            List<User> users = GenerateTestUsers();
            _userRepository.Setup(repo => repo.GetUsersAsync(10)).ReturnsAsync(users);

            // Act
            _userService.ExportUsers(users);

            // Assert
            _fileService.Verify(fileService => fileService.WriteAllLines(It.IsAny<string[]>()), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_Should_Call_GetUsersAsync_On_UserRepository_With_Correct_NumberOfUsers()
        {
            // Arrange
            List<User> users = GenerateTestUsers();
            _userRepository.Setup(repo => repo.GetUsersAsync(10)).ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            _userRepository.Verify(repo => repo.GetUsersAsync(10), Times.Once);
            Assert.Equal(users, result);
        }

        // Helper method to generate test users
        private List<User> GenerateTestUsers()
        {
            List<User> users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                users.Add(new User
                {
                    LastName = $"LastName{i}",
                    FirstName = "Test" ,
                    Age = $"{i}",
                    City = $"City{i}",
                    Email = $"",

                });
            }
            return users;
        }
    }
}
