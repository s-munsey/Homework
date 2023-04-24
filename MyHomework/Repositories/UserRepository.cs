using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyHomework.Config;
using MyHomework.Entities;
using MyHomework.Repositories.Interfaces;
using MyHomework.Services;
using MyHomework.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace MyHomework.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(HttpClient httpClient, IFileService fileService, IOptions<AppSettings> options, ILogger<UserRepository> logger) 
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.Uri);
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<List<User>> GetUsersAsync(int numberOfUsers)
        {
            _logger.LogInformation($"Attempting to fetch {numberOfUsers} users...");
            List<User> users = new List<User>();
            for(int i = 0; i < numberOfUsers; i++)
            {
                var json = await GetRandomUserDataAsync();
                _logger.LogDebug($"parsing {json}");
                var user = ParseUserData(json);
                _logger.LogDebug($"parsed user: {user.FirstName}");
                users.Add(user);
            }

            return users;
        }

        internal async Task<JObject> GetRandomUserDataAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _fileService.AppendAllText(responseBody);
                JObject userData = JObject.Parse(responseBody);
                return userData;
            } catch (Exception ex)
            {
                throw;
            }
            
        }

        internal User ParseUserData(JObject jObject)
        {
            User user = new User
            {
                LastName = jObject["results"][0]["name"]["last"].ToString(),
                FirstName = jObject["results"][0]["name"]["first"].ToString(),
                City = jObject["results"][0]["location"]["city"].ToString(),
                Email = jObject["results"][0]["email"].ToString(),
                Age = jObject["results"][0]["dob"]["age"].ToString()
            };

            return user;
        }
    }
}
