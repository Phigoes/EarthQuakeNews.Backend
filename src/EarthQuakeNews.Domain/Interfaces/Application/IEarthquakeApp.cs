using EarthQuakeNews.Domain.ViewModel;

namespace EarthQuakeNews.Domain.Interfaces.Application
{
    public interface IEarthquakeApp
    {
        Task<IEnumerable<EarthquakeInfoViewModel>> Execute();
        Task<IEnumerable<EarthquakeInfoViewModel>> GetEarthquakeData();
    }
}
