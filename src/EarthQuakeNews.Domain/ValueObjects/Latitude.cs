using EarthQuakeNews.Domain.Common.Base;

namespace EarthQuakeNews.Domain.ValueObjects
{
    public class Latitude : ValueObject
    {
        private Latitude() { }

        public Latitude(double latitude)
        {
            if (latitude is < -90 or > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees inclusive.");

            Degree = latitude;
        }

        public double Degree { get; private init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Degree;
        }
    }
}
