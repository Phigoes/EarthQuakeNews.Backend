using EarthQuakeNews.Domain.DTOs;

namespace EarthQuakeNews.Domain.Interfaces.ExternalServices
{
    public interface IEarthquakeUsgsExternalService
    {
        Task<EarthquakeInfoDto[]> GetEarthquakeToday();
        Task<int> GetEarthquakeCountToday();
    }
}
