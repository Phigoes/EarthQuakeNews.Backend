using EarthQuakeNews.Infra.Polly;
using EarthQuakeNews.Infra.Settings;
using Microsoft.Extensions.Options;

namespace EarthQuakeNews.Infra.HttpClients
{
    public class EarthquakeUsgsClient
    {
        private readonly HttpClient _httpClient;
        private readonly USGSExternalService _usgsExternalService;

        public EarthquakeUsgsClient(HttpClient httpClient, IOptions<RootSettings> options)
        {
            _httpClient = httpClient;
            _usgsExternalService = options.Value.USGSExternalService;
        }

        public async Task<string?> GetEarthquakeToday()
        {
            var startTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var endTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            var url = string.Format($"{_usgsExternalService.Url}{_usgsExternalService.Data}", startTime, endTime);
            var response = await _httpClient.GetAsyncWithRetry(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }

        public async Task<string?> GetEarthquakeCountToday()
        {
            var startTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var endTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            var url = string.Format($"{_usgsExternalService.Url}{_usgsExternalService.Count}", startTime, endTime);
            var response = await _httpClient.GetAsyncWithRetry(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }
    }
}
