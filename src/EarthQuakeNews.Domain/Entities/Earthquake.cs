using EarthQuakeNews.Domain.Common.Base;
using EarthQuakeNews.Domain.Common;
using EarthQuakeNews.Domain.ViewModel;

namespace EarthQuakeNews.Domain.Entities
{
    public class Earthquake : Entity, IAuditable, ISoftDelete, IAggregateRoot
    {
        public Earthquake(double magnitude, string place, double latitude, double longitude, double kmDepth, DateTime earthquakeTime, string code, string url)
        {
            Magnitude = magnitude;
            Place = place;
            Latitude = latitude;
            Longitude = longitude;
            KmDepth = kmDepth;
            EarthquakeTime = earthquakeTime;
            Code = code;
            Url = url;
        }

        public double Magnitude { get; private set; }
        public string Place { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double KmDepth { get; private set; }
        public DateTime EarthquakeTime { get; private set; }
        public string Code { get; private set; }
        public string Url { get; private set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }
        public bool Deleted { get; set; }

        public EarthquakeInfoViewModel ToViewModel()
        {
            return new EarthquakeInfoViewModel
            {
                Magnitude = Magnitude,
                Place = Place,
                Latitude = Math.Round(Latitude, 3),
                Longitude = Math.Round(Longitude, 3),
                KmDepth = KmDepth,
                EarthquakeTime = EarthquakeTime,
                Url = Url
            };
        }
    }
}
