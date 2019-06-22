using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VacuumCleaner.Services;
using VacuumCleaner.Services.Implementation;

namespace VacuumCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                serviceProvider.GetRequiredService<App>().Run();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Exception on top level, stopping execution");
                throw;
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<App>();
            collection.AddSingleton<ISettingsParser, SettingsParser>();
            collection.AddSingleton<IRobotRunner, RobotRunner>();
            collection.AddSingleton<ICommandExecutor, CommandExecutor>();
            collection.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Trace));
            return collection.BuildServiceProvider();
        }
    }
}
