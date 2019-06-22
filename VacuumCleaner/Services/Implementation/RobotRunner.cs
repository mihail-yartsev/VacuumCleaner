using System.Collections.Immutable;
using System.Linq;
using VacuumCleaner.Models;

namespace VacuumCleaner.Services.Implementation
{
    internal class RobotRunner : IRobotRunner
    {
        private readonly ICommandExecutor _commandExecutor;

        public RobotRunner(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public int Clean(SessionSettings settings)
        {
            return settings.Commands
                .Aggregate(new Session(settings.StartingPoint, ImmutableHashSet.Create(settings.StartingPoint)),
                    (session, command) => _commandExecutor.Execute(command, session))
                .CleanedPlaces.Count;
        }
    }
}