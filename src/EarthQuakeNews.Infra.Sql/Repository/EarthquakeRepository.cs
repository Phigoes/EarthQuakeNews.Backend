using EarthQuakeNews.Application.Interfaces.Repositories;
using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Infra.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EarthQuakeNews.Infra.Sql.Repository
{
    public class EarthquakeRepository : GenericRepository<Earthquake>, IEarthquakeRepository
    {
        private readonly EarthQuakeNewsSqlContext _dbContext;

        public EarthquakeRepository(EarthQuakeNewsSqlContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Earthquake>> GetEarthquakes()
        {
            var earthquakes = await _dbContext.Earthquakes
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return earthquakes;
        }

        public async Task SaveListAsync(List<Earthquake> earthquakes)
        {
            _dbContext.Earthquakes.AddRange(earthquakes);
            await _dbContext.SaveChangesAsync();
        }
    }
}
