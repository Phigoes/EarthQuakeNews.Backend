using EarthQuakeNews.Domain.Entities;

namespace EarthQuakeNews.Application.Interfaces.Repositories
{
    public interface IEarthquakeCountRepository
    {
        Task<EarthquakeCount?> GetCountToday();
        Task Save(EarthquakeCount earthquakeCount);
        Task Update(int count);
    }
}
