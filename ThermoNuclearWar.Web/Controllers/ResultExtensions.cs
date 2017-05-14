using IO.Swagger.Model;

namespace ThermoNuclearWar.Web.Controllers
{
    public static class WarheadLaunchResultFactory {
        public static WarheadLaunchResult Fail(string reason)
        {
            return new WarheadLaunchResult(WarheadLaunchResult.ResultEnum.Failure, reason);
        }
    }
}