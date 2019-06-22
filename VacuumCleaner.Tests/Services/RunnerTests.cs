using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using MoqTestSuit;
using VacuumCleaner.Models;
using VacuumCleaner.Services;
using VacuumCleaner.Services.Implementation;
using VacuumCleaner.Tests.TestHelpers;
using Xunit;

namespace VacuumCleaner.Tests.Services
{
    public class RunnerTests
    {
        private static readonly ITestSuit<RobotRunner> _suit = TestSuit.Create<RobotRunner>();

        public RunnerTests()
        {
            _suit.Reset();
        }

        [Fact]
        public void CalledWithSomeSimpleSettings_ShouldCleanAndCountCleanedPlaces()
        {
            // Arrange
            var startingPoint = new Point(10, 10);
            var robotSettings = new SessionSettings(startingPoint, new List<Command>
            {
                new Command(CompassDirection.North, 1),
                new Command(CompassDirection.East, 1),
                new Command(CompassDirection.South, 1),
                new Command(CompassDirection.West, 1),
            });
            var expectedSession = new Session(startingPoint, ImmutableHashSet.Create(startingPoint));
            var cleanedPlacesInTheEnd = Enumerable.Range(0, 10).Select(i => new Point(0, i)).ToImmutableHashSet();
            var expectedSessionOnEnd = new Session(new Point(1, 1), cleanedPlacesInTheEnd);

            _suit.SetupMock<ICommandExecutor>(e => 
                // Make the runner emulate first 3 commands did nothing
                e.Execute(It.Is<Command>(c => robotSettings.Commands.Contains(c)), expectedSession.Equivalent()) == expectedSession && 
                // But the last one returned 10 cleaned places
                e.Execute(robotSettings.Commands[3], expectedSession.Equivalent()) == expectedSessionOnEnd);

            // Act
            var result = _suit.Sut.Clean(robotSettings);

            // Assert
            result.Should().Be(cleanedPlacesInTheEnd.Count, 
                " anyway only the cleanedPlacesAtTheEnd.Count should matter"); 
        }
    }
}
