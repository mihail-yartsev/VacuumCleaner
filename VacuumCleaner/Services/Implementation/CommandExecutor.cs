using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Extensions.Logging;
using VacuumCleaner.Models;

namespace VacuumCleaner.Services.Implementation
{
    internal class CommandExecutor : ICommandExecutor
    {
        private readonly ILogger<CommandExecutor> _logger;

        public CommandExecutor(ILogger<CommandExecutor> logger)
        {
            _logger = logger;
        }

        public Session Execute(Command command, Session session)
        {
            var currentPosition = session.CurrentPosition; 
            var cleanedPlaces = session.CleanedPlaces.ToBuilder();
            
            foreach (var step in GetStepsToExecute(command))
            {
                currentPosition += step;
                var placeWasDusty = cleanedPlaces.Add(currentPosition);
                _logger.LogDebug("Went to ({currentPosition}), it was {placeWasDusty}dusty", session.CurrentPosition,
                    placeWasDusty ? "" : "not ");
            }

            return new Session(currentPosition, cleanedPlaces.ToImmutable());
        }

        private static IEnumerable<Size> GetStepsToExecute(Command command)
        {
            var step = command.Direction switch
            {
                CompassDirection.North => new Size(0, 1),
                CompassDirection.East => new Size(1, 0),
                CompassDirection.South => new Size(0, -1),
                CompassDirection.West => new Size(-1, 0),
                CompassDirection d => throw new NotSupportedException($"Direction {d} is not supported")
            };
            return Enumerable.Repeat(step, command.StepsCount);
        }
    }
}
