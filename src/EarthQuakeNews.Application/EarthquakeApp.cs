using EarthQuakeNews.Application.Interfaces.ExternalServices;
using EarthQuakeNews.Application.Interfaces.Repositories;
using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Domain.Interfaces.Application;
using EarthQuakeNews.Domain.ViewModel;
using Microsoft.Extensions.Logging;

namespace EarthQuakeNews.Application
{
    public class EarthquakeApp : IEarthquakeApp
    {
        private readonly IEarthquakeUsgsExternalService _earthquakeUsgsExternalService;
        private readonly IEarthquakeRepository _earthquakeRepository;
        private readonly IEarthquakeCountRepository _earthquakeCountRepository;
        private readonly ILogger<EarthquakeApp> _logger;

        public EarthquakeApp(IEarthquakeUsgsExternalService earthquakeUsgsExternalService,
            IEarthquakeRepository earthquakeRepository,
            IEarthquakeCountRepository earthquakeCountRepository,
            ILogger<EarthquakeApp> logger)
        {
            _earthquakeUsgsExternalService = earthquakeUsgsExternalService;
            _earthquakeRepository = earthquakeRepository;
            _earthquakeCountRepository = earthquakeCountRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<EarthquakeInfoViewModel>> Execute()
        {
            var earthquakeCountTodayExternalService = await _earthquakeUsgsExternalService.GetEarthquakeCountToday();
            var earthquakeCountTodayDatabase = await _earthquakeCountRepository.GetCountToday();

            if (earthquakeCountTodayDatabase is null)
            {
                _logger.LogInformation("Saving initial earthquakes counting.");
                await _earthquakeCountRepository.Save(new EarthquakeCount(earthquakeCountTodayExternalService));
            }
            else
            {
                if (earthquakeCountTodayExternalService > earthquakeCountTodayDatabase.Count)
                {
                    _logger.LogInformation("Updating earthquakes counting.");
                    await _earthquakeCountRepository.Update(earthquakeCountTodayExternalService);
                }
            }

            if (earthquakeCountTodayExternalService == earthquakeCountTodayDatabase?.Count)
            {
                _logger.LogInformation("Earthquakes counting is the same as USGS Earthquakes counting.");
                var eartquakes = await _earthquakeRepository.GetEarthquakes();
                return eartquakes
                    .Select(data => data.ToViewModel())
                    .OrderByDescending(data => data.EarthquakeTime); ;
            }

            var earthquakeDto = await _earthquakeUsgsExternalService.GetEarthquakeToday();
            var earthquakeData = await _earthquakeRepository.GetEarthquakes();

            if (!earthquakeData.Any())
            {
                _logger.LogInformation("Saving initial earthquakes data.");
                await _earthquakeRepository.SaveListAsync(earthquakeDto.Select(e => e.ToEntity()).ToList());
            }
            else
            {
                var featureIdNotInDb = earthquakeDto
                    .Select(e => e.FeatureId)
                    .Except(earthquakeData.Select(d => d.FeatureId))
                    .ToList();

                var newEarthquakes = earthquakeDto
                    .Where(e => featureIdNotInDb.Contains(e.FeatureId))
                    .ToList();

                _logger.LogInformation($"Saving {newEarthquakes.Count} new earthquake(s) data.");

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
