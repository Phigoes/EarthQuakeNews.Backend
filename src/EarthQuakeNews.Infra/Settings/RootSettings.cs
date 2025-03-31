namespace EarthQuakeNews.Infra.Settings
{
    public class RootSettings
    {
        public ConnectionString ConnectionString { get; set; }
        public Hangfire Hangfire { get; set; }
    }

    public class ConnectionString
    {
        public string SqlServer { get; set; }
    }

    public class Hangfire
    {
        public int IntervalHours { get; set; }
    }
}
