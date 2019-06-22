using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;

namespace VacuumCleaner.Tests.TestHelpers
{
    internal static class ObjectExtensions
    {
        public static T Equivalent<T>(this T o)
        {
            return o.Equivalent(op => op);
        }

        public static T Equivalent<T>(this T o,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
        {
            return Match.Create<T>(s =>
            {
                try
                {
                    s.Should().BeEquivalentTo(o, config);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            });
        }
    }
}