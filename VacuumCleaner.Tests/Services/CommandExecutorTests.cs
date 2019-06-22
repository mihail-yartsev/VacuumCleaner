using System.Collections.Immutable;
using System.Drawing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MoqTestSuit;
using VacuumCleaner.Models;
using VacuumCleaner.Services.Implementation;
using Xunit;

namespace VacuumCleaner.Tests.Services
{
    public class CommandExecutorTests
    {
        private static readonly ITestSuit<CommandExecutor> _suit = TestSuit.Create<CommandExecutor>();
        private static readonly Point _initialPosition = new Point(10, 10);

        public CommandExecutorTests()
        {
            _suit.Reset();
            _suit.SetDependencyToLooseMock<ILogger<CommandExecutor>>();
        }

        [Theory]
        [InlineData(CompassDirection.North, 10, 11)]
        [InlineData(CompassDirection.East, 11, 10)]
        [InlineData(CompassDirection.South, 10, 9)]
        [InlineData(CompassDirection.West, 9, 10)]
        public void OneStepOrdered_ShouldCleanOneStepInTheRightDirection(CompassDirection direction, int newX, int newY)
        {
            // Arrange
            var originalSession = new Session(_initialPosition, ImmutableHashSet.Create(_initialPosition));
            var robotCommand = new Command(direction, 1);

            // Act
            var result = _suit.Sut.Execute(robotCommand, originalSession);

            // Assert
            var newPosition = new Point(newX, newY);
            result.Should().BeEquivalentTo(
                new Session(newPosition, originalSession.CleanedPlaces.Add(newPosition)));
        }

        [Fact]
        public void SeveralStepsOrdered_ShouldCleanAccordingly()
        {
            // Arrange
            var originalSession = new Session(_initialPosition, ImmutableHashSet.Create(_initialPosition));
            var robotCommand = new Command(CompassDirection.North, 3);

            // Act
            var result = _suit.Sut.Execute(robotCommand, originalSession);

            // Assert
            var newPosition = new Point(10, 13);
            result.Should().BeEquivalentTo(
                new Session(newPosition, originalSession.CleanedPlaces
                    .Add(new Point(10, 11))
                    .Add(new Point(10, 12))
                    .Add(newPosition)));
        }

        [Fact]
        public void PlaceWasNotDusty_ShouldNotCleanItAgain()
        {
            // Arrange
            var originalSession = new Session(_initialPosition, 
                ImmutableHashSet.Create(_initialPosition).Add(new Point(10, 11)));
            var robotCommand = new Command(CompassDirection.North, 1);

            // Act
            var result = _suit.Sut.Execute(robotCommand, originalSession);

            // Assert
            var newPosition = new Point(10, 11);
            result.Should().BeEquivalentTo(
                new Session(newPosition, originalSession.CleanedPlaces));
        }
    }
}