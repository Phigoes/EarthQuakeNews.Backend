using EarthQuakeNews.Application;
using EarthQuakeNews.Application.Interfaces.ExternalServices;
using EarthQuakeNews.Application.Interfaces.Repositories;
using EarthQuakeNews.Domain.Interfaces.Application;
using EarthQuakeNews.Infra.ExternalServices;
using EarthQuakeNews.Infra.HttpClients;
using EarthQuakeNews.Infra.HttpClients.Interfaces;
using EarthQuakeNews.Infra.Settings;
using EarthQuakeNews.Infra.Sql.Context;
using EarthQuakeNews.Infra.Sql.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EarthQuakeNews.Infra.DI
{
    public static class DependencyInjection
    {
        private static readonly string Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            var configuration = CreateConfiguration();
            services.Configure<RootSettings>(configuration);

            var rootSettings = configuration.Get<RootSettings>();

            //Application
            services.AddScoped<IEarthquakeApp, EarthquakeApp>();

            //External Services
            services.AddScoped<IEarthquakeUsgsExternalService, EarthquakeUsgsExternalService>();

            //HttpClients
            services.AddHttpClient<IEarthquakeUsgsClient, EarthquakeUsgsClient>(client =>
            {
                client.BaseAddress = new Uri(rootSettings.UsgsExternalService.Url);
            });

            //SQL Server
            var sqlServerConnectionString = rootSettings.ConnectionString.SqlServer;

            services.AddDbContextFactory<EarthQuakeNewsSqlContext>(options =>
                options.UseSqlServer(sqlServerConnectionString));

            //Repositories
            services.AddScoped<IEarthquakeRepository, EarthquakeRepository>();
            services.AddScoped<IEarthquakeCountRepository, EarthquakeCountRepository>();

            //Cache
            services.AddMemoryCache();

            //HealthCheck
            services.AddHealthChecks().AddSqlServer(
                name: "SQL | EarthQuakeNews",
                failureStatus: HealthStatus.Unhealthy,
                connectionString: rootSettings.ConnectionString.SqlServer);

            services.AddHealthChecks().AddUrlGroup(
                name: "API | Earthquake USGS",
                failureStatus: HealthStatus.Unhealthy,
                uri: new Uri($"{rootSettings.UsgsExternalService.Url}/count?format=geojson"));

            return services;
        }

        private static IConfiguration CreateConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            SetConfigurationBuilder(configuration);

            return configuration
                .AddEnvironmentVariables()
                .Build();
        }

        private static void SetConfigurationBuilder(IConfigurationBuilder configuration)
        {
            const string appSettings = "appsettings.json";
            const string appSettingsDevelopment = "appsettings.Development.json";

            if (Environment == "Development")
            {
                if (File.Exists(appSettingsDevelopment))
                {
                    configuration.AddJsonFile(appSettingsDevelopment, optional: false, reloadOnChange: true);
                }
            }
            else
            {
                if (File.Exists(appSettings))
                {
                    configuration.AddJsonFile(appSettings, optional: false, reloadOnChange: true);
                }
            }
        }
    }
}
