using System.Threading.Tasks;
using IO.Swagger.Model;

namespace ThermoNuclearWar.Service
{
    public interface IWarheadsService
    {
        Task<bool> IsOffline();
        Task<WarheadLaunchResult> Launch(string passphrase);
    }
}