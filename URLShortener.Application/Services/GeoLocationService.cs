using System.Net.Http.Json;
using URLShortener.Application.Interfaces;

namespace URLShortener.Application.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;

        public GeoLocationService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://ip-api.com/json/");
        }


        public async Task<string> GetCountryFromIp(string ipAddress)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IpApiResponse>($"{ipAddress}?fields=country");
                return response?.Country ?? "Unknown";
            }
            catch (Exception)
            {

                return "Unknown";
            }
        }

        private class IpApiResponse
        {
            public string Country { get; set; }
        }
    }
}
