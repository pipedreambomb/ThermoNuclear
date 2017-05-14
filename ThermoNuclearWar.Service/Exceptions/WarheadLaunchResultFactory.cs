using IO.Swagger.Model;

namespace ThermoNuclearWar.Service.Exceptions
{
    public static class WarheadLaunchResultFactory {
        public static WarheadLaunchResult Fail(string reason)
        {
            return new WarheadLaunchResult(WarheadLaunchResult.ResultEnum.Failure, reason);
        }
    }
}