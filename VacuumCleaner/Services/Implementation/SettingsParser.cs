using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VacuumCleaner.Models;

namespace VacuumCleaner.Services.Implementation  
{
    internal class SettingsParser : ISettingsParser
    {
        public SessionSettings ParseSettings(IReadOnlyList<string> settingsLines)
        {
            var stepsNumber = int.Parse(settingsLines[0]);
            var startingPoint = ParseStartCoordinates(settingsLines[1]);
            return new SessionSettings(startingPoint, ParseCommands(settingsLines, stepsNumber));
        }

        private static List<Command> ParseCommands(IReadOnlyList<string> settingsLines, int stepsNumber)
        {
            return settingsLines.Skip(2).Take(stepsNumber).Select(ParseSingleCommand).ToList();
        }

        private static Command ParseSingleCommand(string commandLine)
        {
            var splittedLine = commandLine.Split(' ');
            return new Command(ParseCompassDirection(splittedLine[0]), int.Parse(splittedLine[1]));
        }

        private static CompassDirection ParseCompassDirection(string str)
        {
            return str switch
            {
                "N" => CompassDirection.North,
                "E" => CompassDirection.East,
                "S" => CompassDirection.South,
                "W" => CompassDirection.West,
                _ => throw new ArgumentException(str + " is not a valid robot direction", nameof(str))
            };
        }

        private static Point ParseStartCoordinates(string coordinatesLine)
        {
            var startCoordinates = coordinatesLine.Split(' ').Select(int.Parse).ToList();
            return new Point(startCoordinates[0], startCoordinates[1]);
        }

    }
}
