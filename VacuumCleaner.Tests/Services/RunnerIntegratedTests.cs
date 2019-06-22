using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MoqTestSuit;
using VacuumCleaner.Models;
using VacuumCleaner.Services;
using VacuumCleaner.Services.Implementation;
using Xunit;

namespace VacuumCleaner.Tests.Services
{
    public class RunnerIntegratedTests
    {
        private static readonly ITestSuit<RobotRunner> _suit = TestSuit.Create<RobotRunner>();

        public RunnerIntegratedTests()
        {
            _suit.Reset();

            // RobotRunner has a dependency to ICommandExecutor which is set up to be an instance 
            // of the real CommandExecutor with ILogger<CommandExecutor> dependency to be a loose mock
            _suit.SetDependencyToTestSuit<ICommandExecutor, CommandExecutor>();
            _suit.SetupDependencySuit<ICommandExecutor>(s =>
                s.SetDependencyToLooseMock<ILogger<CommandExecutor>>());
        }

        [Fact]
        public void CalledWithSomeSimpleSettings_ShouldCleanAndCountCleanedPlaces()
        {
            // Arrange
            var robotSettings = new SessionSettings(new Point(10, 10), new List<Command>
            {
                new Command(CompassDirection.North, 1),
                new Command(CompassDirection.East, 1),
                new Command(CompassDirection.South, 1),
                new Command(CompassDirection.West, 1),
            });

            // Act
            var result = _suit.Sut.Clean(robotSettings);

            // Assert
            result.Should().Be(4);
        }

        [Fact]
        public void CalledWithSettingsFromExample_ShouldCleanAndCountCleanedPlaces()
        {
            // Arrange
            var robotSettings = new SessionSettings(new Point(10, 22), new List<Command>
            {
                new Command(CompassDirection.East, 2),
                new Command(CompassDirection.North, 1),
            });

            // Act
            var result = _suit.Sut.Clean(robotSettings);

            // Assert
            result.Should().Be(4);
        }

        [Fact]
        public void CalledWithMoreComplexCommands_ShouldCleanAndCountCleanedPlaces()
        {
            // Arrange
            var robotSettings = new SessionSettings(new Point(10, 10), new List<Command>
            {
                new Command(CompassDirection.East, 10), // 11
                new Command(CompassDirection.West, 13), // 14
                new Command(CompassDirection.North, 1), // 15
                new Command(CompassDirection.South, 3), // 17
            });

            // Act
            var result = _suit.Sut.Clean(robotSettings);

            // Assert
            result.Should().Be(17);
        }
    }
}