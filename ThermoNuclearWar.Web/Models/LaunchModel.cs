using System;
using System.ComponentModel.DataAnnotations;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace ThermoNuclearWar.Web.Models
{
    public class LaunchModel
    {
        [Required]
        public string Passphrase { get; set; }
        public bool ServiceIsOffline { get; set; }
        public WarheadLaunchResult LaunchResult { get; set; }
    }
}