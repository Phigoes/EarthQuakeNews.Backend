using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EarthQuakeNews.Infra.Sql.Context
{
    public class EarthQuakeNewsSqlContextFactory : IDesignTimeDbContextFactory<EarthQuakeNewsSqlContext>
    {
        private static readonly string Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        public EarthQuakeNewsSqlContext CreateDbContext(string[] args)
        {
            var configuration = CreateConfiguration();
            var sqlServerConnectionString = configuration.GetSection("ConnectionString:SqlServer").Value;

            var optionsBuilder = new DbContextOptionsBuilder<EarthQuakeNewsSqlContext>();
            optionsBuilder.UseSqlServer(sqlServerConnectionString);

            return new EarthQuakeNewsSqlContext(optionsBuilder.Options);
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
