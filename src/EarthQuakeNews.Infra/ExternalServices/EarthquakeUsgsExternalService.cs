using System.Text.Json;
using System.Text.Json.Nodes;
using EarthQuakeNews.Application.Interfaces.ExternalServices;
using EarthQuakeNews.Domain.DTOs;
using EarthQuakeNews.Infra.HttpClients.Interfaces;

namespace EarthQuakeNews.Infra.ExternalServices
{
    public class EarthquakeUsgsExternalService : IEarthquakeUsgsExternalService
    {
        private readonly IEarthquakeUsgsClient _earthquakeUsgsClient;

        public EarthquakeUsgsExternalService(IEarthquakeUsgsClient earthquakeUsgsClient)
        {
            _earthquakeUsgsClient = earthquakeUsgsClient;
        }

        public async Task<EarthquakeInfoDto[]> GetEarthquakeToday()
        {
            var result = await _earthquakeUsgsClient.GetEarthquakeToday();
            var json = JsonNode.Parse(result);

            var earthquakeInfoDto = json["features"];

            return JsonSerializer.Deserialize<EarthquakeInfoDto[]>(earthquakeInfoDto);
        }

        public async Task<int> GetEarthquakeCountToday()
        {
            var result = await _earthquakeUsgsClient.GetEarthquakeCountToday();

            return JsonDocument.Parse(result).RootElement.GetProperty("count").GetInt32();
        }
    }
}
