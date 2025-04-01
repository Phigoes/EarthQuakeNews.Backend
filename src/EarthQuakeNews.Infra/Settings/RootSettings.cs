namespace EarthQuakeNews.Infra.Settings
{
    public class RootSettings
    {
        public ConnectionStringSettings ConnectionString { get; set; }
        public USGSExternalServiceSettings UsgsExternalService { get; set; }
    }

    public class ConnectionStringSettings
    {
        public string SqlServer { get; set; }
    }

    public class USGSExternalServiceSettings
    {
        public string Url { get; set; }
        public string Data { get; set; }
        public string Count { get; set; }
    }
}
