using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyHomework.Config;
using MyHomework.Services.Interfaces;

namespace MyHomework.Services
{
    public class FileService : IFileService
    {
        private string _responseFileName;
        private string _userFileName;
        private readonly ILogger<FileService> _logger;

        public FileService(IOptions<AppSettings> options, ILogger<FileService> logger)
        {
            _userFileName = options.Value.UserFilePath;
            _responseFileName = options.Value.ResponseFilePath;
            _logger = logger;
        }
        public void AppendAllText(string contents)
        {
            _logger.LogInformation($"Appending {contents}");
            File.AppendAllText(_responseFileName, contents);
        }

        public void WriteAllLines(string[] contents)
        {
            _logger.LogInformation($"Serializing {contents}");
            string jsonString = JsonSerializer.Serialize(contents, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _logger.LogInformation($"Writing to file: {contents}");
            File.WriteAllText(_userFileName, jsonString);
        }
    }
}
