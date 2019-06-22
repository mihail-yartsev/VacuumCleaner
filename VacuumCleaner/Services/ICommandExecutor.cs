using VacuumCleaner.Models;

namespace VacuumCleaner.Services
{
    internal interface ICommandExecutor
    {
        Session Execute(Command command, Session session);
    }
}