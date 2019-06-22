using System.Collections.Generic;
using VacuumCleaner.Models;

namespace VacuumCleaner.Services
{
    internal interface ISettingsParser
    {
        SessionSettings ParseSettings(IReadOnlyList<string> settingsLines);
    }
}