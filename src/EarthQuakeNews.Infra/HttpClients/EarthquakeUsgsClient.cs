using EarthQuakeNews.Infra.Polly;

namespace EarthQuakeNews.Infra.HttpClients
{
    public class EarthquakeUsgsClient
    {
        private readonly HttpClient _httpClient;

        public EarthquakeUsgsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetEarthquakeToday()
        {
            var startTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var endTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            var url = $"query?format=geojson&starttime={startTime}&endtime={endTime}&eventtype=earthquake";
            var response = await _httpClient.GetAsyncWithRetry(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }

        public async Task<string?> GetEarthquakeCountToday()
        {
            var startTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var endTime = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            var url = $"count?format=geojson&starttime={startTime}&endtime={endTime}&eventtype=earthquake";
            var response = await _httpClient.GetAsyncWithRetry(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }
    }
}
