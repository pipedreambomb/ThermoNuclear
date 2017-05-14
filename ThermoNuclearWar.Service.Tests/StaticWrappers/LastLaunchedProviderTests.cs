using System;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ThermoNuclearWar.Service.StaticWrappers;

namespace ThermoNuclearWar.Service.Tests.StaticWrappers
{
    public class LastLaunchedProviderTests
    {
        public class LastLaunched
        {
            [Test]
            public void It_remembers_date_across_instances()
            {
                var expected = new Fixture().Create<DateTime>();
                new LastLaunchedProvider {LastLaunched = expected};
                var secondInstance = new LastLaunchedProvider();
                secondInstance.LastLaunched.Should().Be(expected);
            }
        }
    }
}
