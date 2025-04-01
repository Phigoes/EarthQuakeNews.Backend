using EarthQuakeNews.Infra.HttpClients.Interfaces;
using EarthQuakeNews.Infra.Polly;
using EarthQuakeNews.Infra.Settings;
using Microsoft.Extensions.Options;

namespace EarthQuakeNews.Infra.HttpClients
{
    public class EarthquakeUsgsClient : IEarthquakeUsgsClient
    {
        private readonly HttpClient _httpClient;
        private readonly USGSExternalServiceSettings _usgsExternalServiceSettings;

        public EarthquakeUsgsClient(HttpClient httpClient, IOptions<RootSettings> options)
        {
            _httpClient = httpClient;
            _usgsExternalServiceSettings = options.Value.UsgsExternalService;
        }

        public async Task<string?> GetEarthquakeToday()
        {
            var startTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var endTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            var url = string.Format($"{_usgsExternalServiceSettings.Url}{_usgsExternalServiceSettings.Data}", startTime, endTime);
            var response = await _httpClient.GetAsyncWithRetry(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }

        public async Task<string?> GetEarthquakeCountToday()
        {
            var startTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var endTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            var url = string.Format($"{_usgsExternalServiceSettings.Url}{_usgsExternalServiceSettings.Count}", startTime, endTime);
            var response = await _httpClient.GetAsyncWithRetry(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }
    }
}
