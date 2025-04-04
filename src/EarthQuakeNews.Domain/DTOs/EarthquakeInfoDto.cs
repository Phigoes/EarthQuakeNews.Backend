using System.Text.Json.Serialization;
using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Domain.ValueObjects;

namespace EarthQuakeNews.Domain.DTOs
{
    public record EarthquakeInfoDto
    {
        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }
        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
        [JsonPropertyName("id")]
        public string FeatureId { get; set; }

        public Earthquake ToEntity()
        {
            return new Earthquake(
                magnitude: Properties.Mag,
                place: Properties.Place,
                latitude: new Latitude(Geometry.Coordinates[1]),
                longitude: new Longitude(Geometry.Coordinates[0]),
                kmDepth: Geometry.Coordinates[2],
                earthquakeTime: DateTimeOffset.FromUnixTimeMilliseconds(Properties.Time).UtcDateTime,
                featureId: FeatureId,
                url: Properties.Url);
        }
    }

    public record Properties
    {
        [JsonPropertyName("mag")]
        public double Mag { get; set; }
        [JsonPropertyName("place")]
        public string Place { get; set; }
        [JsonPropertyName("time")]
        public long Time { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public record Geometry
    {
        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }
    }
}
