using EarthQuakeNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EarthQuakeNews.Infra.Sql.Context
{
    public class EarthQuakeNewsSqlContext : DbContext
    {
        public EarthQuakeNewsSqlContext(DbContextOptions<EarthQuakeNewsSqlContext> options) : base(options) { }

        public EarthQuakeNewsSqlContext() { }

        public virtual DbSet<Earthquake> Earthquakes { get; set; }
        public virtual DbSet<EarthquakeCount> EarthquakesCount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Earthquake>().HasQueryFilter(p => !p.Deleted);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EarthQuakeNewsSqlContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
