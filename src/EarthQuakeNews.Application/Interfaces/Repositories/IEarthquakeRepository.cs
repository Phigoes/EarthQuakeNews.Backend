using EarthQuakeNews.Domain.Entities;

namespace EarthQuakeNews.Application.Interfaces.Repositories
{
    public interface IEarthquakeRepository : IGenericRepository<Earthquake>
    {
        Task<IEnumerable<Earthquake>> GetEarthquakes();
        Task SaveListAsync(List<Earthquake> earthquakes);
    }
}
