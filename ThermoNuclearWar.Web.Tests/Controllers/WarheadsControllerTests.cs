using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using FluentAssertions.Mvc;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ThermoNuclearWar.Service;
using ThermoNuclearWar.Web.Controllers;
using ThermoNuclearWar.Web.Models;

namespace ThermoNuclearWar.Web.Tests.Controllers
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

        protected void AssertModelStateHasErrorForPropertyNamed(string name)
        {
            SUT.ModelState[name].Should().NotBeNull();
            SUT.ModelState[name].Errors.Should().HaveCount(1);
        }

        public class Launch_GET : WarheadsControllerTests
        {
            [Test]
            public async Task It_returns_the_default_view()
            {
                // Act
                ActionResult actual = await SUT.Launch();

                // Assert
                actual.Should()
                      .BeViewResult()
                      .WithDefaultViewName();
            }

            [Test]
            public async Task It_returns_a_model()
            {
                // Act
                ActionResult actual = await SUT.Launch();

                // Assert
                object actualModel = actual.Should()
                                           .BeViewResult()
                                           .Model;
                actualModel.Should().NotBeNull();
                actualModel.Should().BeOfType<LaunchModel>();
            }

            [TestCase(true)]
            [TestCase(false)]
            public async Task It_sets_model_status_from_service(bool isOffline)
            {
                // Arrange
                _warheadsService.IsOffline()
                                .Returns(isOffline);

                // Act
                ActionResult actual = await SUT.Launch();

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
                public async Task It_adds_model_error()
                {
                    // Arrange
                    _warheadsService.IsOffline().Returns(true);

                    // Act
                    ActionResult actual = await SUT.Launch(new LaunchModel());

                    // Assert
                    actual.GetModel().ServiceError.Should().Be("Service is offline.");
                }
            }

            public class When_status_is_online_and_passcode_is_wrong : WarheadsControllerTests
            {
                [Test]
                public async Task It_adds_model_error()
                {
                    // Arrange
                    _warheadsService.IsOffline().Returns(false);
                    _warheadsService.WhenForAnyArgs(ws => ws.Launch(""))
                                    .Throw<WrongPassphraseException>();

                    // Act
                    await SUT.Launch(new LaunchModel());

                    // Assert
                    AssertModelStateHasErrorForPropertyNamed(nameof(LaunchModel.Passphrase));
                }
            }

            public class When_status_is_online_and_passcode_is_correct_and_service_call_works : WarheadsControllerTests
            {
                [Test]
                public async Task It_returns_view_with_no_errors()
                {
                    // Arrange
                    string correctPassphrase = new Fixture().Create<string>();
                    _warheadsService.IsOffline().Returns(false);
                    _warheadsService.When(ws => ws.Launch(Arg.Is<string>(@string => @string != correctPassphrase)))
                                    .Throw<WrongPassphraseException>();

                    // Act
                    ActionResult actual = await SUT.Launch(new LaunchModel {Passphrase = correctPassphrase});

                    // Assert
                    actual.Should().BeViewResult();
                    SUT.ModelState.Should().BeEmpty();
                }
            }
            public class When_status_is_online_and_passcode_is_correct_and_service_call_fails : WarheadsControllerTests
            {
                [Test]
                public async Task It_returns_model_with_ServiceError_from_service()
                {
                    // Arrange
                    string expectedErrorMessage = new Fixture().Create<string>();
                    _warheadsService.IsOffline().Returns(false);
                    _warheadsService.WhenForAnyArgs(ws => ws.Launch(null))
                                    .Throw(new WarheadsApiException(expectedErrorMessage));

                    // Act
                    ActionResult actual = await SUT.Launch(new LaunchModel());

                    // Assert
                    actual.GetModel().ServiceError.Should().Be(expectedErrorMessage);
                }
            }

            public class When_already_launched_in_last_five_minutes : WarheadsControllerTests
            {
                [Test]
                public async Task It_adds_model_error()
                {
                    // Act
                    _warheadsService.IsOffline().Returns(false);
                    _warheadsService.WhenForAnyArgs(ws => ws.Launch(null))
                                    .Throw<AlreadyLaunchedException>();
                    // Act
                    await SUT.Launch(new LaunchModel());
                    ActionResult actual = await SUT.Launch(new LaunchModel());

                    // Assert
                    actual.GetModel().ServiceError.Should().Be(
                                                               "Too soon to launch again. Please allow 5 minutes to elapse.");
                }
            }

            public class When_already_launched_in_over_five_minutes : WarheadsControllerTests
            {
                [Test]
                public async Task It_works()
                {
                    // Act
                    await SUT.Launch(new LaunchModel());
                    ActionResult actual = await SUT.Launch(new LaunchModel());

                    // Assert
                    actual.GetModel().ServiceError.Should().BeNullOrEmpty();
                }
            }
        }
    }
}