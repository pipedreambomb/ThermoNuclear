using System;
using System.Threading.Tasks;
using IO.Swagger.Api;
using IO.Swagger.Model;
using static IO.Swagger.Model.WarheadStatusResult.StatusEnum;

namespace ThermoNuclearWar.Service
{
    public class WarheadsService : IWarheadsService
    {
        internal const string CorrectPassphrase = "NICEGAMEOFCHESS";
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
            var status = await _apiClient.WarheadsGetStatusAsync();
            return status.Status == Offline;
        }

        public async Task Launch(string passphrase)
        {
            if(passphrase != CorrectPassphrase) throw new WrongPassphraseException();

            if(_lastLaunchedProvider.LastLaunched > DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5)))
                throw new AlreadyLaunchedException();

            var result = await _apiClient.WarheadsLaunchAsync(passphrase);

            if (result.Result == WarheadLaunchResult.ResultEnum.Failure)
                throw new WarheadsApiException(result.Message);
        }
    }
}