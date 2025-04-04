using EarthQuakeNews.Domain.DTOs;

namespace EarthQuakeNews.Application.Interfaces.ExternalServices
{
    public interface IEarthquakeUsgsExternalService
    {
        Task<EarthquakeInfoDto[]> GetEarthquakeToday();
        Task<int> GetEarthquakeCountToday();
    }
}
