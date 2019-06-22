using System.Collections.Immutable;
using System.Drawing;

namespace VacuumCleaner.Models
{
    internal class Session
    {
        public Point CurrentPosition { get; }
        public ImmutableHashSet<Point> CleanedPlaces { get; }

        public Session(Point currentPosition, ImmutableHashSet<Point> cleanedPlaces)
        {
            CurrentPosition = currentPosition;
            CleanedPlaces = cleanedPlaces;
        }
    }
}