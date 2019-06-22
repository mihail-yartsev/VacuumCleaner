namespace VacuumCleaner.Models
{
    internal class Command
    {
        public CompassDirection Direction { get; }
        public int StepsCount { get; }

        public Command(CompassDirection direction, int stepsCount)
        {
            Direction = direction;
            StepsCount = stepsCount;
        }
    }
}