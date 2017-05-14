using System;
using System.Threading.Tasks;
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

        [HttpGet]
        public async Task<ActionResult> Launch()
        {
            return View(new LaunchModel {ServiceIsOffline = await _warheadsService.IsOffline()});
        }

        [HttpPost]
        public async Task<ActionResult> Launch(LaunchModel model)
        {
            if (await _warheadsService.IsOffline())
            {
                model.ServiceError = "Service is offline.";
                return View(model);
            }
            try
            {
                _warheadsService.Launch(model.Passphrase);
            }
            catch (WrongPassphraseException)
            {
                ModelState.AddModelError(nameof(model.Passphrase), "Wrong passphrase.");
                return View(model);
            }
            catch (AlreadyLaunchedException)
            {
                model.ServiceError = "Too soon to launch again. Please allow 5 minutes to elapse.";
                return View(model);
            }
            catch (WarheadsApiException exception)
            {
                model.ServiceError = exception.Message;
                return View(model);
            }

            return View(model);
        }
    }
}