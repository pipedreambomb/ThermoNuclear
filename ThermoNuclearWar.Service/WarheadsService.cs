using System;

namespace ThermoNuclearWar.Service
{
    public class WarheadsService : IWarheadsService {
        public bool IsOffline()
        {
            return true;
        }

        public void Launch(string passphrase)
        {
            throw new NotImplementedException();
        }
    }
}