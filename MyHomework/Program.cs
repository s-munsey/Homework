using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHomework.Repositories;
using MyHomework.Repositories.Interfaces;
using MyHomework.Config;
using MyHomework.Services.Interfaces;
using MyHomework.Services;
using System;
using MyHomework;
using System.CommandLine;
using Microsoft.Extensions.Logging;

static void ConfigureServices(IServiceCollection services)
{
    // build config
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    services.Configure<AppSettings>(configuration.GetSection("App"));
    services.AddHttpClient();

    // add services:
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IFileService, FileService>();
    services.AddScoped<IUserRepository, UserRepository>();
    var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.ClearProviders(); // Clear any existing logging providers
        builder.AddConsole(); // Add Console logger
    });

    // add logger factory to services
    services.AddSingleton<ILoggerFactory>(loggerFactory);

    // add app
    services.AddTransient<App>();
}

var services = new ServiceCollection();
ConfigureServices(services);

// create service provider
using var serviceProvider = services.BuildServiceProvider();

// entry to run app
await serviceProvider.GetService<App>().InvokeAsync(args);
        
    
