using System.Threading.Tasks;
using System.Web.Mvc;
using ThermoNuclearWar.Service;
using ThermoNuclearWar.Service.Exceptions;
using ThermoNuclearWar.Web.Models;

namespace ThermoNuclearWar.Web.Controllers
{
    public class WarheadsController : Controller
    {
        public const string AlreadyLaunchedErrorMessage = "Too soon to launch again. Please allow 5 minutes to elapse, in order to avoid premature detonation and the concomitant annihilation of Gitland.";
        private const string WrongPassphrase = "Wrong passphrase.";
        public const string ServiceIsOffline = "Service is offline.";

        private readonly IWarheadsService _warheadsService;

        // Poor man's dependency injection, as it didn't seem worth setting
        // up an IoC container for this simple site.
        public WarheadsController() : this(new WarheadsService()) {}

        public WarheadsController(IWarheadsService warheadsService)
        {
            _warheadsService = warheadsService;
        }

        [HttpGet]
        public async Task<ActionResult> Launch()
        {
            return View(new LaunchModel {ServiceIsOffline = await _warheadsService.IsOffline()});
        }

        [HttpPost]
        public async Task<ActionResult> Launch(LaunchModel model)
        {
            if(ModelState.IsValid == false) return View(model);

            if (await _warheadsService.IsOffline())
            {
                model.LaunchResult = WarheadLaunchResultFactory.Fail(ServiceIsOffline);
                model.ServiceIsOffline = true;
                return View(model);
            }

            try
            {
                model.LaunchResult = await _warheadsService.Launch(model.Passphrase);
            }
            catch (WrongPassphraseException)
            {
                ModelState.AddModelError(nameof(model.Passphrase), WrongPassphrase);
                return View(model);
            }
            catch (AlreadyLaunchedException)
            {
                model.LaunchResult = WarheadLaunchResultFactory.Fail(AlreadyLaunchedErrorMessage);
                return View(model);
            }

            return View(model);
        }
    }
}