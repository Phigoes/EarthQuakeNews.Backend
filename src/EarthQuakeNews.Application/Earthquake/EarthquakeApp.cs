using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Domain.Interfaces.Application;
using EarthQuakeNews.Domain.Interfaces.ExternalServices;
using EarthQuakeNews.Domain.Interfaces.Repositories;
using EarthQuakeNews.Domain.ViewModel;
using Microsoft.Extensions.Logging;

namespace EarthQuakeNews.Application.Earthquake
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
                _logger.LogInformation("Saving earthquake couting.");
                await _earthquakeCountRepository.Save(new EarthquakeCount(earthquakeCountTodayExternalService));
            }
            else
            {
                if (earthquakeCountTodayExternalService > earthquakeCountTodayDatabase.Count)
                {
                    _logger.LogInformation("Updating earthquake couting.");
                    await _earthquakeCountRepository.Update(earthquakeCountTodayExternalService);
                }
            }

            if (earthquakeCountTodayExternalService == earthquakeCountTodayDatabase?.Count)
            {
                _logger.LogInformation("Earthquake counting is the same as USGS Earthquake couting.");
                var eartquakes = await _earthquakeRepository.GetEarthquakes();
                return eartquakes
                    .Select(data => data.ToViewModel())
                    .OrderByDescending(data => data.EarthquakeTime); ;
            }

            var earthquakeDto = await _earthquakeUsgsExternalService.GetEarthquakeToday();
            var earthquakeData = await _earthquakeRepository.GetEarthquakes();

            if (!earthquakeData.Any())
            {
                _logger.LogInformation("Saving earthquake data.");
                await _earthquakeRepository.SaveListAsync(earthquakeDto.Select(e => e.ToEntity()).ToList());
            }
            else
            {
                var codeNotInDb = earthquakeDto
                    .Select(e => e.Properties.Code)
                    .Except(earthquakeData.Select(d => d.Code))
                    .ToList();

                var newEarthquakes = earthquakeDto
                    .Where(e => codeNotInDb.Contains(e.Properties.Code, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                _logger.LogInformation($"Saving {newEarthquakes.Count} new earthquake data.");

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
