using EarthQuakeNews.Domain.Entities;

namespace EarthQuakeNews.Domain.Interfaces.Repositories
{
    public interface IEarthquakeRepository : IGenericRepository<Earthquake>
    {
        Task<IEnumerable<Earthquake>> GetEarthquakes();
        Task SaveListAsync(List<Earthquake> earthquakes);
    }
}
