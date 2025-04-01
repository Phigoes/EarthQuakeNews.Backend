namespace EarthQuakeNews.Domain.ViewModel
{
    public record EarthquakeInfoViewModel
    {
        public double Id { get; set; }
        public double Magnitude { get; set; }
        public string Place { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double KmDepth { get; set; }
        public DateTime EarthquakeTime { get; set; }
        public string Url { get; set; }
    }
}
