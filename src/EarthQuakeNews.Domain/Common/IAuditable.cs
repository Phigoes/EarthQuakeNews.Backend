namespace EarthQuakeNews.Domain.Common
{
    internal interface IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
