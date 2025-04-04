using EarthQuakeNews.Application;
using EarthQuakeNews.Application.Interfaces.ExternalServices;
using EarthQuakeNews.Application.Interfaces.Repositories;
using EarthQuakeNews.Domain.DTOs;
using EarthQuakeNews.Domain.Entities;
using EarthQuakeNews.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace EarthQuakeNews.UnitTest.Application
{
    public class EarthquakeAppTests
    {
        private readonly EarthquakeApp _earthquakeApp;
        private readonly Mock<IEarthquakeUsgsExternalService> _earthquakeUsgsExternalServiceMock = new();
        private readonly Mock<IEarthquakeRepository> _earthquakeRepositoryMock = new();
        private readonly Mock<IEarthquakeCountRepository> _earthquakeCountRepositoryMock = new();
        private readonly Mock<ILogger<EarthquakeApp>> _loggerMock = new();

        public EarthquakeAppTests()
        {
            _earthquakeApp = new EarthquakeApp(
                _earthquakeUsgsExternalServiceMock.Object,
                _earthquakeRepositoryMock.Object,
                _earthquakeCountRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        [Trait("App", "Earthquake")]
        public async Task GetEarthquakeData__HasData_MustReturnEarthquakeData()
        {
            var data = GetEartquakesFromDb();

            _earthquakeRepositoryMock
                .Setup(e => e.GetEarthquakes())
                .ReturnsAsync(data);

            var result = await _earthquakeApp.GetEarthquakeData();

            Assert.NotEmpty(result);
        }

        [Fact]
        [Trait("App", "Earthquake")]
        public async Task GetEarthquakeData__HasNoData_MustSaveEarthquakesData()
        {
            _earthquakeRepositoryMock
                .SetupSequence(e => e.GetEarthquakes())
                .ReturnsAsync(new List<Earthquake>())
                .ReturnsAsync(new List<Earthquake>())
                .ReturnsAsync(GetEartquakesFromDb());
            _earthquakeUsgsExternalServiceMock
                .Setup(e => e.GetEarthquakeCountToday())
                .ReturnsAsync(3);
            _earthquakeCountRepositoryMock
                .Setup(e => e.GetCountToday())
                .ReturnsAsync((EarthquakeCount?)null);
            _earthquakeUsgsExternalServiceMock
                .Setup(e => e.GetEarthquakeToday())
                .ReturnsAsync(GetEartquakesFromExternalService());

            var result = await _earthquakeApp.GetEarthquakeData();

            Assert.NotEmpty(result);

            _earthquakeCountRepositoryMock.Verify(e => e.Save(It.IsAny<EarthquakeCount>()), Times.Once);
            _earthquakeRepositoryMock.Verify(e => e.SaveListAsync(It.IsAny<List<Earthquake>>()), Times.Once);
        }

        [Fact]
        [Trait("App", "Earthquake")]
        public async Task Execute__HasNoData_MustSaveEarthquakesData()
        {
            _earthquakeRepositoryMock
                .SetupSequence(e => e.GetEarthquakes())
                .ReturnsAsync(new List<Earthquake>())
                .ReturnsAsync(GetEartquakesFromDb());
            _earthquakeUsgsExternalServiceMock
                .Setup(e => e.GetEarthquakeCountToday())
                .ReturnsAsync(3);
            _earthquakeCountRepositoryMock
                .Setup(e => e.GetCountToday())
                .ReturnsAsync((EarthquakeCount?)null);
            _earthquakeUsgsExternalServiceMock
                .Setup(e => e.GetEarthquakeToday())
                .ReturnsAsync(GetEartquakesFromExternalService());

            var result = await _earthquakeApp.Execute();

            Assert.NotEmpty(result);

            _earthquakeCountRepositoryMock.Verify(e => e.Save(It.IsAny<EarthquakeCount>()), Times.Once);
            _earthquakeRepositoryMock.Verify(e => e.SaveListAsync(It.IsAny<List<Earthquake>>()), Times.Once);
        }

        [Fact]
        [Trait("App", "Earthquake")]
        public async Task Execute__HasDifferentCountData_MustReturnUpdatedEarthquakeData()
        {
            _earthquakeRepositoryMock
                .SetupSequence(e => e.GetEarthquakes())
                .ReturnsAsync(GetEartquakesFromDb())
                .ReturnsAsync(GetEartquakesFromDbNewData());
            _earthquakeUsgsExternalServiceMock
                .Setup(e => e.GetEarthquakeCountToday())
                .ReturnsAsync(4);
            _earthquakeCountRepositoryMock
                .Setup(e => e.GetCountToday())
                .ReturnsAsync(new EarthquakeCount(3));
            _earthquakeUsgsExternalServiceMock
                .Setup(e => e.GetEarthquakeToday())
                .ReturnsAsync(GetEartquakesFromExternalServiceNewData());

            var result = await _earthquakeApp.Execute();

            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count());

            _earthquakeCountRepositoryMock.Verify(e => e.Update(It.IsAny<int>()), Times.Once);
            _earthquakeRepositoryMock.Verify(e => e.SaveListAsync(It.IsAny<List<Earthquake>>()), Times.Once);
        }

        [Fact]
        [Trait("App", "Earthquake")]
        public async Task Execute__HasSameCountData_MustReturnEarthquakeData()
        {
            _earthquakeRepositoryMock
                .SetupSequence(e => e.GetEarthquakes())
                .ReturnsAsync(GetEartquakesFromDb());
            _earthquakeUsgsExternalServiceMock
                .Setup(e => e.GetEarthquakeCountToday())
                .ReturnsAsync(3);
            _earthquakeCountRepositoryMock
                .Setup(e => e.GetCountToday())
                .ReturnsAsync(new EarthquakeCount(3));

            var result = await _earthquakeApp.Execute();

            Assert.NotEmpty(result);
        }

        private static IEnumerable<Earthquake> GetEartquakesFromDb()
        {
            return new List<Earthquake>
            {
                new(0.76, "10 km WNW of The Geysers, CA", new Latitude(38.806835), new Longitude(-122.865669), 1.3500, DateTime.UtcNow, "nc75158552", "https://earthquake.usgs.gov/earthquakes/eventpage/nc75158552"),
                new(0.90, "35 km NNW of Beluga, Alaska", new Latitude(61.440900), new Longitude(-151.331000), 64.8000, DateTime.UtcNow, "ak02546rfm3u", "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546rfm3u"),
                new(0.76, "66 km NNE of Petersville, Alaska", new Latitude(63.061500), new Longitude(-150.342900), 105.5000, DateTime.UtcNow, "ak02546r3kto", "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546r3kto")
            };
        }

        private static EarthquakeInfoDto[] GetEartquakesFromExternalService()
        {
            return
            [
                new EarthquakeInfoDto
                {
                    Properties = new Properties
                    {
                        Mag = 0.76,
                        Place = "10 km WNW of The Geysers, CA",
                        Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Url = "https://earthquake.usgs.gov/earthquakes/eventpage/nc75158552"
                    },
                    Geometry = new Geometry
                    {
                        Coordinates = [-122.865669, 38.806835, 1.3500]
                    },
                    FeatureId = "nc75158552"
                },
                new EarthquakeInfoDto
                {
                    Properties = new Properties
                    {
                        Mag = 0.90,
                        Place = "35 km NNW of Beluga, Alaska",
                        Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Url = "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546rfm3u"
                    },
                    Geometry = new Geometry
                    {
                        Coordinates = [-151.331000, 61.440900, 64.8000]
                    },
                    FeatureId = "ak02546rfm3u"
                },
                new EarthquakeInfoDto
                {
                    Properties = new Properties
                    {
                        Mag = 0.76,
                        Place = "66 km NNE of Petersville, Alaska",
                        Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Url = "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546r3kto"
                    },
                    Geometry = new Geometry
                    {
                        Coordinates = [-150.342900, 63.061500, 105.5000]
                    },
                    FeatureId = "ak02546r3kto"
                }
            ];
        }

        private static IEnumerable<Earthquake> GetEartquakesFromDbNewData()
        {
            return new List<Earthquake>
            {
                new(0.76, "10 km WNW of The Geysers, CA", new Latitude(38.806835), new Longitude(-122.865669), 1.3500, DateTime.UtcNow, "nc75158552", "https://earthquake.usgs.gov/earthquakes/eventpage/nc75158552"),
                new(0.90, "35 km NNW of Beluga, Alaska", new Latitude(61.440900), new Longitude(-151.331000), 64.8000, DateTime.UtcNow, "ak02546rfm3u", "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546rfm3u"),
                new(0.76, "66 km NNE of Petersville, Alaska", new Latitude(63.061500), new Longitude(-150.342900), 105.5000, DateTime.UtcNow, "ak02546r3kto", "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546r3kto"),
                new(1.80, "30 km NW of Toyah, Texas", new Latitude(31.520000), new Longitude(-104.011000), 7.5867, DateTime.UtcNow, "2025gjqh", "https://earthquake.usgs.gov/earthquakes/eventpage/2025gjqh"),
            };
        }

        private static EarthquakeInfoDto[] GetEartquakesFromExternalServiceNewData()
        {
            return
            [
                new EarthquakeInfoDto
                {
                    Properties = new Properties
                    {
                        Mag = 0.76,
                        Place = "10 km WNW of The Geysers, CA",
                        Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Url = "https://earthquake.usgs.gov/earthquakes/eventpage/nc75158552"
                    },
                    Geometry = new Geometry
                    {
                        Coordinates = [-122.865669, 38.806835, 1.3500]
                    },
                    FeatureId = "nc75158552"
                },
                new EarthquakeInfoDto
                {
                    Properties = new Properties
                    {
                        Mag = 0.90,
                        Place = "35 km NNW of Beluga, Alaska",
                        Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Url = "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546rfm3u"
                    },
                    Geometry = new Geometry
                    {
                        Coordinates = [-151.331000, 61.440900, 64.8000]
                    },
                    FeatureId = "ak02546rfm3u"
                },
                new EarthquakeInfoDto
                {
                    Properties = new Properties
                    {
                        Mag = 0.76,
                        Place = "66 km NNE of Petersville, Alaska",
                        Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Url = "https://earthquake.usgs.gov/earthquakes/eventpage/ak02546r3kto"
                    },
                    Geometry = new Geometry
                    {
                        Coordinates = [-150.342900, 63.061500, 105.5000]
                    },
                    FeatureId = "ak02546r3kto"
                },
                new EarthquakeInfoDto
                {
                    Properties = new Properties
                    {
                        Mag = 1.80,
                        Place = "30 km NW of Toyah, Texas",
                        Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Url = "https://earthquake.usgs.gov/earthquakes/eventpage/2025gjqh"
                    },
                    Geometry = new Geometry
                    {
                        Coordinates = [-104.011000, 31.520000, 7.5867]
                    },
                    FeatureId = "2025gjqh"
                }
            ];
        }
    }
}
