namespace EarthQuakeNews.Infra.HttpClients.Interfaces
{
    public interface IEarthquakeUsgsClient
    {
        Task<string?> GetEarthquakeToday();
        Task<string?> GetEarthquakeCountToday();
    }
}
