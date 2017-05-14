using System;

namespace ThermoNuclearWar.Service
{
    public class WarheadsApiException : Exception
    {
        public WarheadsApiException(string message) : base(message)
        {
        }
    }
}