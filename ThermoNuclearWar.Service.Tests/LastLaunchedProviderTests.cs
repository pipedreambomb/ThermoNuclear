using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace ThermoNuclearWar.Service.Tests
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
