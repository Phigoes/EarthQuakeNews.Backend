using EarthQuakeNews.Application.Earthquake;
using EarthQuakeNews.Domain.Interfaces.Application;
using EarthQuakeNews.Domain.Interfaces.ExternalServices;
using EarthQuakeNews.Domain.Interfaces.Repositories;
using EarthQuakeNews.Infra.ExternalServices;
using EarthQuakeNews.Infra.HttpClients;
using EarthQuakeNews.Infra.Settings;
using EarthQuakeNews.Infra.Sql.Context;
using EarthQuakeNews.Infra.Sql.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EarthQuakeNews.Infra.DI
{
    public static class DependencyInjection
    {
        private static readonly string Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            var configuration = CreateConfiguration();
            var connectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionString>();

            //Application
            services.AddScoped<IEarthquakeApp, EarthquakeApp>();

            //External Services
            services.AddScoped<IEarthquakeUsgsExternalService, EarthquakeUsgsExternalService>();

            //HttpClients
            services.AddHttpClient<EarthquakeUsgsClient>(client =>
            {
                client.BaseAddress = new Uri("https://earthquake.usgs.gov/fdsnws/event/1/");
            });

            //Repositories
            var sqlServerConnectionString = connectionStrings.SqlServer;

            services.AddDbContextFactory<EarthQuakeNewsSqlContext>(options =>
                options.UseSqlServer(sqlServerConnectionString));

            services.AddScoped<IEarthquakeRepository, EarthquakeRepository>();
            services.AddScoped<IEarthquakeCountRepository, EarthquakeCountRepository>();

            //Cache



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
