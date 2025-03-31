namespace EarthQuakeNews.Domain.Common
{
    internal interface ISoftDelete
    {
        public bool Deleted { get; set; }
    }
}
