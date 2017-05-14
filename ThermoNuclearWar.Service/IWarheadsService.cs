using System.Threading.Tasks;

namespace ThermoNuclearWar.Service
{
    public interface IWarheadsService
    {
        Task<bool> IsOffline();
        void Launch(string passphrase);
    }
}