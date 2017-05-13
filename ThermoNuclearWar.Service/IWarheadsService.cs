namespace ThermoNuclearWar.Service
{
    public interface IWarheadsService
    {
        bool IsOffline();
        void Launch(string passphrase);
    }
}