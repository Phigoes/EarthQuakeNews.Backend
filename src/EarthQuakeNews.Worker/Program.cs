using EarthQuakeNews.Infra.IoC.DI;
using EarthQuakeNews.Infra.Job;
using EarthQuakeNews.Infra.Settings;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Options;

namespace EarthQuakeNews.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDependencies();

            //.Services.AddHangfire(config => config.UseInMemoryStorage());

            builder.Services.AddHangfire(config =>
            {
                using var provider = builder.Services.BuildServiceProvider();
                var rootSettings = provider.GetService<IOptions<RootSettings>>();

                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(rootSettings.Value.ConnectionString.SqlServer,
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            DisableGlobalLocks = true,
                        });
            });
            builder.Services.AddHangfireServer();
            builder.Services.AddHostedService<EarthquakeJob>();
            
            var host = builder.Build();

            host.Run();
        }
    }
}