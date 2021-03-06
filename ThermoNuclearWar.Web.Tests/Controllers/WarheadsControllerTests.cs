using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using FluentAssertions.Mvc;
using IO.Swagger.Model;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ThermoNuclearWar.Service;
using ThermoNuclearWar.Service.Exceptions;
using ThermoNuclearWar.Web.Controllers;
using ThermoNuclearWar.Web.Models;
using static IO.Swagger.Model.WarheadLaunchResult.ResultEnum;

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
                    AssertLaunchResultFailureWithMessage(actual, WarheadsController.ServiceIsOffline);
                }

                [Test]
                public async Task It_sets_model_ServiceIsOffline_to_true()
                {
                    // Arrange
                    _warheadsService.IsOffline().Returns(true);

                    // Act
                    ActionResult actual = await SUT.Launch(new LaunchModel());

                    // Assert
                    actual.GetModel().ServiceIsOffline.Should().BeTrue();
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
            public class When_status_is_online_and_service_call_fails : WarheadsControllerTests
            {
                [Test]
                public async Task It_returns_model_with_ServiceError_from_service()
                {
                    // Arrange
                    var expectedResult = new Fixture().Create<WarheadLaunchResult>();

                    _warheadsService.IsOffline().Returns(false);
                    _warheadsService.Launch(null)
                                    .ReturnsForAnyArgs(Task.FromResult(expectedResult));

                    // Act
                    ActionResult actual = await SUT.Launch(new LaunchModel());

                    // Assert
                    actual.GetModel().LaunchResult.Should().Be(expectedResult);
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
                    actual.GetModel()
                          .LaunchResult.Message.Should()
                          .Be(WarheadsController.AlreadyLaunchedErrorMessage);
                }
            }

            public class When_already_launched_in_over_five_minutes : WarheadsControllerTests
            {
                [Test]
                public async Task It_works()
                {
                    // Arrange
                    // nothing to do here, just don't throw an AlreadyLaunchedException either time

                    // Act
                    await SUT.Launch(new LaunchModel());
                    await SUT.Launch(new LaunchModel());

                    // Assert
                    SUT.ModelState.Should().BeEmpty();
                }
            }
        }

        protected static void AssertLaunchResultFailureWithMessage(ActionResult actual, string message)
        {
            actual.GetModel()
                  .LaunchResult.Result.Should()
                  .Be(Failure);
            actual.GetModel()
                  .LaunchResult.Message.Should()
                  .Be(message);
        }
    }
}