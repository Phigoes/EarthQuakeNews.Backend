using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Domain.Interfaces.Application;
using EarthQuakeNews.Domain.Interfaces.ExternalServices;
using EarthQuakeNews.Domain.Interfaces.Repositories;
using EarthQuakeNews.Domain.ViewModel;

namespace EarthQuakeNews.Application.Earthquake
{
    public class EarthquakeApp : IEarthquakeApp
    {
        private readonly IEarthquakeUsgsExternalService _earthquakeUsgsExternalService;
        private readonly IEarthquakeRepository _earthquakeRepository;
        private readonly IEarthquakeCountRepository _earthquakeCountRepository;

        public EarthquakeApp(IEarthquakeUsgsExternalService earthquakeUsgsExternalService,
            IEarthquakeRepository earthquakeRepository,
            IEarthquakeCountRepository earthquakeCountRepository)
        {
            _earthquakeUsgsExternalService = earthquakeUsgsExternalService;
            _earthquakeRepository = earthquakeRepository;
            _earthquakeCountRepository = earthquakeCountRepository;
        }

        public async Task<IEnumerable<EarthquakeInfoViewModel>> Execute()
        {
            var earthquakeCountTodayExternalService = await _earthquakeUsgsExternalService.GetEarthquakeCountToday();
            var earthquakeCountTodayDatabase = await _earthquakeCountRepository.GetCountToday();

            if (earthquakeCountTodayDatabase is null)
            {
                await _earthquakeCountRepository.Save(new EarthquakeCount(earthquakeCountTodayExternalService));
            }
            else
            {
                if (earthquakeCountTodayExternalService > earthquakeCountTodayDatabase.Count)
                    await _earthquakeCountRepository.Update(earthquakeCountTodayExternalService);
            }

            if (earthquakeCountTodayExternalService == earthquakeCountTodayDatabase?.Count)
            {
                var eartquakes = await _earthquakeRepository.GetEarthquakes();
                return eartquakes
                    .Select(data => data.ToViewModel())
                    .OrderByDescending(data => data.EarthquakeTime); ;
            }

            var earthquakeDto = await _earthquakeUsgsExternalService.GetEarthquakeToday();
            var earthquakeData = await _earthquakeRepository.GetEarthquakes();

            if (!earthquakeData.Any())
            {
                await _earthquakeRepository.SaveListAsync(earthquakeDto.Select(e => e.ToEntity()).ToList());
            }
            else
            {
                var codeNotInDb = earthquakeDto
                    .Select(e => e.Properties.Code)
                    .Except(earthquakeData.Select(d => d.Code))
                    .ToList();

                var newEarthquakes = earthquakeDto
                    .Where(e => codeNotInDb.Contains(e.Properties.Code))
                    .ToList();

                await _earthquakeRepository.SaveListAsync(newEarthquakes.Select(e => e.ToEntity()).ToList());
            }

            earthquakeData = await _earthquakeRepository.GetEarthquakes();
            return earthquakeData
                .Select(data => data.ToViewModel())
                .OrderByDescending(data => data.EarthquakeTime);
        }

        public async Task<IEnumerable<EarthquakeInfoViewModel>> GetEarthquakeData()
        {
            var earthquakeData = await _earthquakeRepository.GetEarthquakes();

            if (!earthquakeData.Any())
                return await Execute();

            return earthquakeData
                .Select(data => data.ToViewModel())
                .OrderByDescending(data => data.EarthquakeTime); ;
        }
    }
}
