using System;
using System.Web.Mvc;
using FluentAssertions;
using FluentAssertions.Mvc;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ThermoNuclearWar.Controllers;
using ThermoNuclearWar.Models;

namespace ThermoNuclearWar.Tests.Controllers
{
    [TestFixture]
    public class WarheadsControllerTests
    {
        // System Under Test
        protected WarheadsController SUT;
        protected IWarheadsService _warheadsService;

        [SetUp]
        public void SetUp()
        {
            _warheadsService = Substitute.For<IWarheadsService>();
            SUT = new WarheadsController(_warheadsService);
        }

        public class Launch_GET : WarheadsControllerTests
        {
            [Test]
            public void It_returns_the_default_view()
            {
                // Act
                ActionResult actual = SUT.Launch();

                // Assert
                actual.Should()
                      .BeViewResult()
                      .WithDefaultViewName();
            }

            [Test]
            public void It_returns_a_model()
            {
                // Act
                ActionResult actual = SUT.Launch();

                // Assert
                object actualModel = actual.Should()
                                           .BeViewResult()
                                           .Model;
                actualModel.Should().NotBeNull();
                actualModel.Should().BeOfType<LaunchModel>();
            }

            [TestCase(true)]
            [TestCase(false)]
            public void It_sets_model_status_from_service(bool isOffline)
            {
                // Arrange
                _warheadsService.IsOffline()
                                .Returns(isOffline);

                // Act
                ActionResult actual = SUT.Launch();

                // Assert
                var actualModel = actual.Should()
                                        .BeViewResult()
                                        .Model.As<LaunchModel>();
                actualModel.ServiceIsOffline.Should().Be(isOffline);
            }
        }

        public class Launch_POST 
        {
            public class When_status_is_offline : WarheadsControllerTests
            {
                [Test]
                public void It_throws()
                {
                    // Arrange
                    _warheadsService.IsOffline().Returns(true);

                    // Act
                    Action act = () => SUT.Launch(new LaunchModel());

                    // Assert
                    act.ShouldThrow<WarheadsServiceOfflineException>();
                }
            }

            public class When_status_is_online_and_passcode_is_wrong : WarheadsControllerTests
            {
                [Test]
                public void It_throws()
                {
                    // Arrange
                    _warheadsService.IsOffline().Returns(false);
                    _warheadsService.WhenForAnyArgs(ws => ws.Launch(""))
                                    .Throw<WrongPassphraseException>();

                    // Act
                    Action act = () => SUT.Launch(new LaunchModel());

                    // Assert
                    act.ShouldThrow<WrongPassphraseException>();
                }
            }

            public class When_status_is_online_and_passcode_is_correct : WarheadsControllerTests
            {
                [Test]
                public void It_returns_empty_result()
                {
                    // Arrange
                    string correctPassphrase = new Fixture().Create<string>();
                    _warheadsService.IsOffline().Returns(false);
                    _warheadsService.When(ws => ws.Launch(Arg.Is<string>(@string => @string != correctPassphrase)))
                                    .Throw<WrongPassphraseException>();

                    // Act
                    ActionResult actual = SUT.Launch(new LaunchModel {Passphrase = correctPassphrase});

                    // Assert
                    actual.Should().BeEmptyResult();
                }
            }
        }

        public class When_already_launched_in_last_five_minutes : WarheadsControllerTests
        {
            [Test]
            public void It_throws()
            {
                // Act
                SUT.Launch(new LaunchModel());
                Action act = () => SUT.Launch(new LaunchModel());

                // Assert
                act.ShouldThrow<AlreadyLaunchedException>();
            }
        }
        public class When_already_launched_in_over_five_minutes : WarheadsControllerTests
        {
            [Test]
            public void It_works()
            {
                // Act
                SUT.Launch(new LaunchModel());
                SUT.LastLaunched = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(value: 5)
                                                                    // allow extra time for the test to run
                                                                    .Add(TimeSpan.FromSeconds(value: 10)));
                ;
                Action act = () => SUT.Launch(new LaunchModel());

                // Assert
                act.ShouldNotThrow();
            }
        }
    }
}