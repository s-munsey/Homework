using Microsoft.Extensions.Options;
using MyHomework.Config;
using MyHomework.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using Microsoft.Extensions.Logging;
using MyHomework.Exceptions;

namespace MyHomework
{
    public class App : RootCommand
    {
        private readonly ILogger<App> _logger;
        private readonly IUserService _userService;

        public App(IUserService userService, ILogger<App> logger) :base()
        {
            _logger = logger;
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.SetHandler(Execute);
        }

        private async Task Execute()
        {
            _logger.LogInformation("App Starting...");

            try {
                var users = await _userService.GetUsersAsync();
                _userService.ExportUsers(users);
            } catch (Exception ex)
            {
                if(ex is UserFetchException)
                {
                    _logger.LogError($"Failed to parse response", ex);
                } else
                {
                    _logger.LogError($"Http call failed", ex);
                }
            } 

            _logger.LogInformation("App Finishing...");
            await Task.CompletedTask;
        }
    }
}
