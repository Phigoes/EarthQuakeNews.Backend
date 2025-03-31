using EarthQuakeNews.Domain.Interfaces.Repositories;
using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Infra.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EarthQuakeNews.Infra.Sql.Repository
{
    public class EarthquakeRepository : GenericRepository<Earthquake>, IEarthquakeRepository
    {
        private readonly EarthQuakeNewsSqlContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        private const string KEY_EARTHQUAKES = "Earthquakes";

        public EarthquakeRepository(EarthQuakeNewsSqlContext dbContext, IMemoryCache memoryCache) : base(dbContext)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Earthquake>> GetEarthquakes()
        {
            var earthquakesCache = _memoryCache.Get<List<Earthquake>>(KEY_EARTHQUAKES);

            if (earthquakesCache is not null)
                return earthquakesCache;

            var earthquakes = await _dbContext.Earthquakes
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            _memoryCache.Set(KEY_EARTHQUAKES, earthquakes);

            return earthquakes;
        }

        public async Task SaveListAsync(List<Earthquake> earthquakes)
        {
            _dbContext.Earthquakes.AddRange(earthquakes);
            await _dbContext.SaveChangesAsync();

            _memoryCache.Remove(KEY_EARTHQUAKES);
        }
    }
}
