using System.Collections.Generic;
using System.Drawing;

namespace VacuumCleaner.Models
{
    internal class SessionSettings
    {
        public Point StartingPoint { get; }
        public IReadOnlyList<Command> Commands { get; }

        public SessionSettings(Point startingPoint, IReadOnlyList<Command> commands)
        {
            StartingPoint = startingPoint;
            Commands = commands;
        }
    }
}