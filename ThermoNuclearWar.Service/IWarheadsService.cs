using System.Threading.Tasks;

namespace ThermoNuclearWar.Service
{
    public interface IWarheadsService
    {
        Task<bool> IsOffline();
        Task Launch(string passphrase);
    }
}