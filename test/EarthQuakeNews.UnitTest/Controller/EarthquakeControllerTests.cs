using EarthQuakeNews.Domain.Interfaces.Application;
using EarthQuakeNews.Domain.ViewModel;
using EarthQuakeNews.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EarthQuakeNews.UnitTest.Controller
{
    public class EarthquakeControllerTests
    {
        private readonly EarthquakeController _earthquakeController;
        private readonly Mock<IEarthquakeApp> _earthquakeAppMock = new();

        public EarthquakeControllerTests()
        {
            _earthquakeController = new EarthquakeController();
        }

        [Fact]
        [Trait("Controller", "Earthquake")]
        public async Task Execute__HasData_MustReturnOk()
        {
            var response = GetEartquakes();

            _earthquakeAppMock
                .Setup(x => x.GetEarthquakeData())
                .ReturnsAsync(response);

            var result = await _earthquakeController.GetEarthquakeInfo(_earthquakeAppMock.Object);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        [Trait("Controller", "Earthquake")]
        public async Task Execute__HasNoData_MustReturnNoContent()
        {
            _earthquakeAppMock
                .Setup(x => x.GetEarthquakeData())
                .ReturnsAsync(new List<EarthquakeInfoViewModel>());

            var result = await _earthquakeController.GetEarthquakeInfo(_earthquakeAppMock.Object);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        private static IEnumerable<EarthquakeInfoViewModel> GetEartquakes()
        {
            return new List<EarthquakeInfoViewModel>
            {
                new()
                {
                    Magnitude = 0.76,
                    Place = "10 km WNW of The Geysers, CA",
                    Latitude = 38.806835,
                    Longitude = -122.865669,
                    KmDepth = 1.3500,
                    EarthquakeTime = DateTime.UtcNow,
                    Url = "https://earthquake.usgs.gov/earthquakes/eventpage/nc75158552"
                },
                new()
                {
                    Magnitude = 0.90,
                    Place = "35 km NNW of Beluga, Alaska",
                    Latitude = 61.440900,
                    Longitude = -151.331000,
                    KmDepth = 64.8000,
                    EarthquakeTime = DateTime.UtcNow,
                    Url = "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546rfm3u"
                },
                new()
                {
                    Magnitude = 0.76,
                    Place = "66 km NNE of Petersville, Alaska",
                    Latitude = 63.061500,
                    Longitude = -150.342900,
                    KmDepth = 105.5000,
                    EarthquakeTime = DateTime.UtcNow,
                    Url = "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546r3kto"
                }
            };
        }
    }
}
