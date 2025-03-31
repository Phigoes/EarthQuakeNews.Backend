using EarthQuakeNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EarthQuakeNews.Infra.Sql.Mappings
{
    public class EarthquakesCountMapping : IEntityTypeConfiguration<EarthquakeCount>
    {
        public void Configure(EntityTypeBuilder<EarthquakeCount> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .ToTable("EarthquakesCount");
        }
    }
}
