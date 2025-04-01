using EarthQuakeNews.Domain.Common;
using EarthQuakeNews.Domain.Common.Base;

namespace EarthQuakeNews.Domain.Entities
{
    public class EarthquakeCount : Entity, IAuditable
    {
        public EarthquakeCount(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }

        public void Update(int count)
        {
            Count = count;
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}
