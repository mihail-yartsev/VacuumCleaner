using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using VacuumCleaner.Services;

namespace VacuumCleaner
{
    internal class App
    {
        private readonly ILogger<Program> _logger;
        private readonly ISettingsParser _settingsParser;
        private readonly IRobotRunner _runner;

        public App(ILogger<Program> logger, ISettingsParser settingsParser, IRobotRunner runner)
        {
            _logger = logger;
            _settingsParser = settingsParser;
            _runner = runner;
        }

        public void Run()
        {                
            var inputFileLines = File.ReadAllLines("input.txt");
            var settings = _settingsParser.ParseSettings(inputFileLines);
            var cleanedPlacesCount = _runner.Clean(settings);
            File.WriteAllText("output.txt", "=> Cleaned: " + cleanedPlacesCount);
            _logger.LogInformation("Done, cleaned {count}", cleanedPlacesCount);
        }
    }
}
