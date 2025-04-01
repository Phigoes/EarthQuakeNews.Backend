namespace EarthQuakeNews.Infra.Settings
{
    public class RootSettings
    {
        public ConnectionString ConnectionString { get; set; }
        public USGSExternalService USGSExternalService { get; set; }
    }

    public class ConnectionString
    {
        public string SqlServer { get; set; }
    }

    public class USGSExternalService
    {
        public string Url { get; set; }
        public string Data { get; set; }
        public string Count { get; set; }
    }
}
