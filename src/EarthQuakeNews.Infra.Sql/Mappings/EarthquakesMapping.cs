using EarthQuakeNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EarthQuakeNews.Infra.Sql.Mappings
{
    public class EarthquakesMapping : IEntityTypeConfiguration<Earthquake>
    {
        public void Configure(EntityTypeBuilder<Earthquake> builder)
        {
            builder
                .HasKey(e=> e.Id);

            builder
                .Property(e => e.Magnitude)
                .HasColumnType("decimal")
                .HasPrecision(4, 2);

            builder
                .Property(e => e.Place)
                .HasColumnType("varchar(100)");

            builder.ComplexProperty(e => e.Latitude, latitudeBuilder =>
            {
                latitudeBuilder.Property(l => l.Degree)
                    .HasColumnType("decimal")
                    .HasPrecision(8, 6);
            });

            builder.ComplexProperty(e => e.Longitude, longitudeBuilder =>
            {
                longitudeBuilder.Property(l => l.Degree)
                    .HasColumnType("decimal")
                    .HasPrecision(9, 6);
            });

            builder
                .Property(e => e.KmDepth)
                .HasColumnType("decimal")
                .HasPrecision(7, 4);

            builder
                .Property(e => e.Code)
                .HasColumnType("varchar(20)");

            builder
                .Property(e => e.Url)
                .HasColumnType("varchar(100)");

            builder
                .ToTable("Earthquakes");
        }
    }
}
