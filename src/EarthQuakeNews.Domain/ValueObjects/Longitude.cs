using EarthQuakeNews.Domain.Common.Base;

namespace EarthQuakeNews.Domain.ValueObjects
{
    public class Longitude : ValueObject
    {
        private Longitude() { }

        public Longitude(double longitude)
        {
            if (longitude is < -180 or > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Latitude must be between -90 and 90 degrees inclusive.");

            Degree = longitude;
        }

        public double Degree { get; private init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Degree;
        }
    }
}
