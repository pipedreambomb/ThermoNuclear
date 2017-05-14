using System.Web.Mvc;
using FluentAssertions.Mvc;
using ThermoNuclearWar.Web.Models;

namespace ThermoNuclearWar.Web.Tests.Controllers
{
    public static class ActionResultTestExtensions {
        public static LaunchModel GetModel(this ActionResult actionResult)
        {
            return actionResult.Should().BeViewResult().ModelAs<LaunchModel>();
        }
    }
}