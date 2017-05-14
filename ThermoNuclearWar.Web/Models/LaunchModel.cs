using System;
using System.ComponentModel.DataAnnotations;

namespace ThermoNuclearWar.Web.Models
{
    public class LaunchModel
    {
        [Required]
        public string Passphrase { get; set; }
        public bool ServiceIsOffline { get; set; }
        public string ServiceError { get; set; }
    }
}