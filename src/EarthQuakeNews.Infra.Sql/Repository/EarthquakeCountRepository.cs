using EarthQuakeNews.Application.Interfaces.Repositories;
using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Infra.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace EarthQuakeNews.Infra.Sql.Repository
{
    public class EarthquakeCountRepository : IEarthquakeCountRepository
    {
        private readonly IDbContextFactory<EarthQuakeNewsSqlContext> _dbContextFactory;

        public EarthquakeCountRepository(IDbContextFactory<EarthQuakeNewsSqlContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<EarthquakeCount?> GetCountToday()
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var earthquakeCount = await context.EarthquakesCount
                .AsNoTracking()
                .Where(e => e.CreatedAt.Date == DateTime.UtcNow.Date)
                .FirstOrDefaultAsync();

            return earthquakeCount;
        }

        public async Task Save(EarthquakeCount earthquakeCount)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            context.EarthquakesCount.Add(earthquakeCount);
            await context.SaveChangesAsync();
        }

        public async Task Update(int count)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var earthquakeCountToday = await context.EarthquakesCount
                .AsNoTracking()
                .FirstAsync(e => e.CreatedAt.Date == DateTime.UtcNow.Date);

            earthquakeCountToday.Update(count);

            context.EarthquakesCount.Update(earthquakeCountToday);
            await context.SaveChangesAsync();
        }
    }
}
