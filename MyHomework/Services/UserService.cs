using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyHomework.Config;
using MyHomework.Entities;
using MyHomework.Exceptions;
using MyHomework.Repositories.Interfaces;
using MyHomework.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyHomework.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IFileService _fileService;
        private readonly int _numberOfUsers;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, IFileService fileService, IOptions<AppSettings> options, ILogger<UserService> logger) 
        { 
            _repository = repository;
            _fileService = fileService;
            _numberOfUsers = options.Value.NumberOfUsers;
            _logger = logger;
        }

        public void ExportUsers(List<User> users)
        {
            _logger.LogInformation("Exporting users...");
            string[] output = new string[_numberOfUsers];

            try
            {
                for (int i = 0; i < _numberOfUsers; i++)
                {
                    var user = users[i];
                    _logger.LogDebug($"serializing user: {user.FirstName}");
                    var userJson = JsonSerializer.Serialize(users[i]);
                    output[i] = userJson;
                }
            } catch(Exception ex)
            {
                _logger.LogError($"Failed to serialize user", ex);
            }

            _fileService.WriteAllLines(output);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            _logger.LogInformation("Fetching users...");
            var users = new List<User>();
            try
            {
                users = await _repository.GetUsersAsync(_numberOfUsers);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                if(ex is HttpRequestException || ex is InvalidOperationException || ex is TaskCanceledException)
                {
                    throw;
                }
                throw new UserFetchException("Failed to fetch users", ex);
            }

            return users;
        }
    }
}
