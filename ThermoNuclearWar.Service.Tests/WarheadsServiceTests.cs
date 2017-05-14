using System;
using System.Threading.Tasks;
using FluentAssertions;
using IO.Swagger.Api;
using IO.Swagger.Model;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using static IO.Swagger.Model.WarheadLaunchResult.ResultEnum;
using static IO.Swagger.Model.WarheadStatusResult.StatusEnum;
using static ThermoNuclearWar.Service.WarheadsService;

namespace ThermoNuclearWar.Service.Tests
{
    [TestFixture]
    public class WarheadsServiceTests
    {
        protected WarheadsService SUT;
        protected IWarheadsApi MockApi;
        protected ILastLaunchedProvider MockLastLaunchedProvider;

        [SetUp]
        public void SetUp()
        {
            MockApi = Substitute.For<IWarheadsApi>();
            MockLastLaunchedProvider = Substitute.For<ILastLaunchedProvider>();
            SUT = new WarheadsService(MockApi, MockLastLaunchedProvider);
        }

        public class IsOffline
        {
            public class When_API_returns_offline : WarheadsServiceTests
            {
                [Test]
                public async Task It_returns_true()
                {
                    // Arrange
                    MockApi.WarheadsGetStatusAsync()
                           .Returns(Task.FromResult(new WarheadStatusResult
                                                    {
                                                        Status = Offline
                                                    }));


                    // Act
                    var actual = await SUT.IsOffline();

                    // Assert
                    actual.Should().BeTrue();
                }
            }
            public class When_API_returns_online : WarheadsServiceTests
            {
                [Test]
                public async Task It_returns_false()
                {
                    // Arrange
                    MockApi.WarheadsGetStatusAsync()
                           .Returns(Task.FromResult(new WarheadStatusResult
                                                    {
                                                        Status = Online
                                                    }));


                    // Act
                    var actual = await SUT.IsOffline();

                    // Assert
                    actual.Should().BeFalse();
                }
            }
        }

        public class Launch
        {
            public class When_passphrase_incorrect : WarheadsServiceTests
            {
                [Test]
                public void It_throws()
                {
                    // Arrange
                    const string wrongPassphrase = "wrong wrong wrong";

                    // Act
                    Func<Task> act = async () => await SUT.Launch(wrongPassphrase);

                    // Assert
                    act.ShouldThrow<WrongPassphraseException>();
                }
            }

            public class When_last_launched_within_past_five_minutes : WarheadsServiceTests
            {
                [Test]
                public void It_throws()
                {
                    // Arrange
                    MockLastLaunchedProvider.LastLaunched
                                        .Returns(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5))
                                                                // allow a little extra time for the test to run
                                                                .Add(TimeSpan.FromSeconds(10)));

                    // Act
                    Func<Task> act = async () => await SUT.Launch(CorrectPassphrase);

                    // Assert
                    act.ShouldThrow<AlreadyLaunchedException>();
                }
            }

            public class When_OK_to_launch 
            {
                public class When_service_call_returns_success : WarheadsServiceTests
                {
                    [Test]
                    public void It_does_not_throw()
                    {
                        // Arrange
                        MockApi.WarheadsLaunchAsync(null)
                               .ReturnsForAnyArgs(Task.FromResult(new WarheadLaunchResult
                               {
                                   Result = Success
                               }));
                        // Act
                        Func<Task> act = async () => await SUT.Launch(CorrectPassphrase);

                        // Assert
                        act.ShouldNotThrow();
                    }
                }

                public class When_service_call_returns_failure : WarheadsServiceTests
                {
                    [Test]
                    public void It_throws_with_message_from_result()
                    {
                        // Arrange
                        var expectedErrorMessage = new Fixture().Create<string>();
                        MockApi.WarheadsLaunchAsync(null)
                               .ReturnsForAnyArgs(Task.FromResult(new WarheadLaunchResult
                               {
                                   Result = Failure,
                                   Message = expectedErrorMessage
                               }));

                        // Act
                        Func<Task> act = async () => await SUT.Launch(CorrectPassphrase);

                        // Assert
                        act.ShouldThrow<WarheadsApiException>()
                           .WithMessage(expectedErrorMessage);
                    }
                }
            }
        }
    }
}
