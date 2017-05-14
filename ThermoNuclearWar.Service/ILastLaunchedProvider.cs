using System;

namespace ThermoNuclearWar.Service
{
    /// <summary>
    /// Wrapper for global variable so that tests can run independently.
    /// </summary>
    public interface ILastLaunchedProvider {
        DateTime LastLaunched { get; set; }
    }
}