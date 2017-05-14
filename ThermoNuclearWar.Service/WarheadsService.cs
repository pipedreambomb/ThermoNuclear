using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ThermoNuclearWar.Service
{
    public class WarheadsStatusModel
    {
        public string Status { get; set; }
        public bool IsOffline => Status != "Online";
    }


    public class WarheadsService : IWarheadsService
    {

        private static HttpClient client;

        static WarheadsService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://gitland.azurewebsites.net:80/api/warheads/");
            // accept JSON
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> IsOffline()
        {
                WarheadsStatusModel status = null;
                HttpResponseMessage response = await client.GetAsync("status");
                if (response.IsSuccessStatusCode)
                {
                    status = await response.Content.ReadAsAsync<WarheadsStatusModel>();
                }
                return status.IsOffline;
        }

        public void Launch(string passphrase)
        {
            throw new NotImplementedException();
        }
    }
}