using EarthQuakeNews.Application;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace EarthQuakeNews.Infra.Job
{
    public class EarthquakeJob : BackgroundService
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public EarthquakeJob(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _recurringJobManager.AddOrUpdate<EarthquakeApp>(
                nameof(EarthquakeJob),
                job => job.Execute(),
                Cron.Hourly);

            await Task.CompletedTask;
        }
    }
}
