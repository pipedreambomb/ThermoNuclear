using System;
using System.Web.Mvc;
using ThermoNuclearWar.Service;
using ThermoNuclearWar.Web.Models;

namespace ThermoNuclearWar.Web.Controllers
{
    public class WarheadsController : Controller
    {
        private readonly IWarheadsService _warheadsService;

        // Poor man's dependency injection, as it didn't seem worth setting
        // up an IoC container for this simple site.
        public WarheadsController() : this(new WarheadsService()) {}

        public WarheadsController(IWarheadsService warheadsService)
        {
            _warheadsService = warheadsService;
        }

        // I'm cheating a bit here. It would be better to make this
        // testable by wrapping the static DateTime.UtcNow in a mockable interface.
        internal DateTime? LastLaunched { get; set; }

        [HttpGet]
        public ActionResult Launch()
        {
            return View(new LaunchModel {ServiceIsOffline = _warheadsService.IsOffline()});
        }

        [HttpPost]
        public ActionResult Launch(LaunchModel model)
        {
            if(_warheadsService.IsOffline()) throw new WarheadsServiceOfflineException();
            if(LastLaunched > DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5))) throw new AlreadyLaunchedException();

            _warheadsService.Launch(model.Passphrase);
            LastLaunched = DateTime.UtcNow;

            return new EmptyResult();
        }
    }
}