using System.Collections.Immutable;
using EarthQuakeNews.Domain.Interfaces.Repositories;
using EarthQuakeNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using EarthQuakeNews.Infra.Sql.Context;
using Microsoft.Extensions.Caching.Memory;

namespace EarthQuakeNews.Infra.Sql.Repository
{
    public class EarthquakeRepository : IEarthquakeRepository
    {
        private readonly IDbContextFactory<EarthQuakeNewsSqlContext> _dbContextFactory;
        private readonly IMemoryCache _memoryCache;

        private const string KEY_EARTHQUAKES = "Earthquakes";

        public EarthquakeRepository(IDbContextFactory<EarthQuakeNewsSqlContext> dbContextFactory, IMemoryCache memoryCache)
        {
            _dbContextFactory = dbContextFactory;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Earthquake>> GetEarthquakes()
        {
            var earthquakesCache = _memoryCache.Get<List<Earthquake>>(KEY_EARTHQUAKES);

            if (earthquakesCache is not null)
                return earthquakesCache;

            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var earthquakes = await context.Earthquakes
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            _memoryCache.Set(KEY_EARTHQUAKES, earthquakes);

            return earthquakes;
        }

        public async Task SaveListAsync(List<Earthquake> earthquakes)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Earthquakes.AddRange(earthquakes);
            await context.SaveChangesAsync();

            _memoryCache.Remove(KEY_EARTHQUAKES);
        }
    }
}
