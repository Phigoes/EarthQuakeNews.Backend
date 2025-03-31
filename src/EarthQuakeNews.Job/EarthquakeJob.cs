using EarthQuakeNews.Application.Earthquake;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EarthQuakeNews.Infra.Job
{
    public class EarthquakeJob : BackgroundService
    {
        private readonly ILogger<EarthquakeJob> _logger;
        private readonly IRecurringJobManager _recurringJobManager;

        public EarthquakeJob(ILogger<EarthquakeJob> logger, IRecurringJobManager recurringJobManager)
        {
            _logger = logger;
            _recurringJobManager = recurringJobManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            _recurringJobManager.AddOrUpdate<EarthquakeApp>(
                nameof(EarthquakeJob),
                job => job.Execute(),
                Cron.Hourly);

            await Task.CompletedTask;
        }
    }
}
