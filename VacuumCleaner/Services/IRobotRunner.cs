using VacuumCleaner.Models;

namespace VacuumCleaner.Services
{
    internal interface IRobotRunner
    {
        int Clean(SessionSettings settings);
    }

}