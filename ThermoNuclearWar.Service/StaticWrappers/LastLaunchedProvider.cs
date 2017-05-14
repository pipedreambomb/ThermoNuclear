using System;

namespace ThermoNuclearWar.Service.StaticWrappers
{
    /// <summary>
    /// Static wrapper class to allow unit tests to run independently.
    /// We need to remember this date across instances of the Warheads
    /// service, because it will result in catastrophic failure if
    /// we launch the warheads more than once in a given time period.
    /// </summary>
    public class LastLaunchedProvider : ILastLaunchedProvider
    {
        private static DateTime? _lastLaunched;

        public DateTime? LastLaunched
        {
            get { return _lastLaunched; }
            set { _lastLaunched = value; }
        }
    }
}