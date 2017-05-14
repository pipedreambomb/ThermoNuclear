using System;
using System.Threading.Tasks;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using Newtonsoft.Json;
using ThermoNuclearWar.Service.Exceptions;
using ThermoNuclearWar.Service.StaticWrappers;
using static IO.Swagger.Model.WarheadLaunchResult.ResultEnum;
using static IO.Swagger.Model.WarheadStatusResult.StatusEnum;

namespace ThermoNuclearWar.Service
{
    public class WarheadsService : IWarheadsService
    {
        internal const string CorrectPassphrase = "NICEGAMEOFCHESS";
        internal static string CorrectPasscode => $"{DateTime.UtcNow:yyMMdd}-{CorrectPassphrase}";

        private readonly IWarheadsApi _apiClient;
        private readonly ILastLaunchedProvider _lastLaunchedProvider;

        // Poor man's dependency injection, as it didn't seem worth setting
        // up an IoC container for this simple site.
        public WarheadsService() : this(new WarheadsApi(), new LastLaunchedProvider()) {}

        public WarheadsService(IWarheadsApi apiClient, ILastLaunchedProvider lastLaunchedProvider)
        {
            _apiClient = apiClient;
            _lastLaunchedProvider = lastLaunchedProvider;
        }

        public async Task<bool> IsOffline()
        {
            WarheadStatusResult status = await _apiClient.WarheadsGetStatusAsync();
            return status.Status == Offline;
        }

        public async Task<WarheadLaunchResult> Launch(string passphrase)
        {
            if(passphrase != CorrectPassphrase) throw new WrongPassphraseException();

            if(_lastLaunchedProvider.LastLaunched > DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5)))
                throw new AlreadyLaunchedException();

            try
            {
                await _apiClient.WarheadsLaunchAsync(CorrectPasscode);
            }
            catch (ApiException exception)
            {
                return JsonConvert.DeserializeObject<WarheadLaunchResult>(exception.ErrorContent);
            }
            // no exception, therefore a success
            _lastLaunchedProvider.LastLaunched = DateTime.UtcNow;
            return new WarheadLaunchResult { Result = Success};
        }
    }
}