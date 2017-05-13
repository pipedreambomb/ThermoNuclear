using ThermoNuclearWar.Models;

namespace ThermoNuclearWar.Controllers
{
    public interface IWarheadsService
    {
        bool IsOffline();
        void Launch(string passphrase);
    }
}