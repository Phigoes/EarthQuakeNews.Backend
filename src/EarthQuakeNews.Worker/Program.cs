using EarthQuakeNews.Infra.DI;
using EarthQuakeNews.Infra.Job;
using Hangfire;

namespace EarthQuakeNews.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDependencies();
            builder.Services.AddMemoryCache();
            builder.Services.AddHangfire(config => config.UseInMemoryStorage());
            builder.Services.AddHangfireServer();

            builder.Services.AddHostedService<EarthquakeJob>();
            
            var host = builder.Build();

            host.Run();
        }
    }
}