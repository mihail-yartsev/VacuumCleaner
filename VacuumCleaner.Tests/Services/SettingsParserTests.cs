using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using MoqTestSuit;
using VacuumCleaner.Models;
using VacuumCleaner.Services.Implementation;
using Xunit;

namespace VacuumCleaner.Tests.Services
{
    public class SettingsParserTests
    {
        private static readonly ITestSuit<SettingsParser> _suit = TestSuit.Create<SettingsParser>();

        public SettingsParserTests()
        {
            _suit.Reset();
        }

        public static IEnumerable<object[]> GetTheoryData()
        {
            // Should correctly parse a normal file with different directions
            yield return new object[]
            {
                @"4\10 10\N 2\E 1\S 3\W 4".Split('\\'),
                new SessionSettings(new Point(10, 10), new List<Command>
                {
                    new Command(CompassDirection.North, 2),
                    new Command(CompassDirection.East, 1),
                    new Command(CompassDirection.South, 3),
                    new Command(CompassDirection.West, 4),
                })
            };

            // If sent more commands than expected just take what was expected
            yield return new object[]
            {
                @"2\10 10\N 2\E 1\W 4".Split('\\'),
                new SessionSettings(new Point(10, 10), new List<Command>
                {
                    new Command(CompassDirection.North, 2),
                    new Command(CompassDirection.East, 1),
                })
            };

            // If sent less commands than expected just take what's there
            yield return new object[]
            {
                @"10\10 10\N 2\E 1".Split('\\'),
                new SessionSettings(new Point(10, 10), new List<Command>
                {
                    new Command(CompassDirection.North, 2),
                    new Command(CompassDirection.East, 1),
                })
            };
        }

        [Theory]
        [MemberData(nameof(GetTheoryData))]
        public void CalledWithSomeSimpleSettings_ShouldCleanAndCountCleanedPlaces(string[] file,
            object expectedSettings)
        {
            // Arrange

            // Act
            var settings = _suit.Sut.ParseSettings(file);

            // Assert
            settings.Should().BeEquivalentTo(expectedSettings);
        }
    }
}