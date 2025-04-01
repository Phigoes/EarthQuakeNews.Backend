using EarthQuakeNews.Domain.Interfaces.Application;
using EarthQuakeNews.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuakeNews.WebApi.Controllers
{
    [ApiController]
    [Route("api/earthquakes")]
    public class EarthquakeController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<IEnumerable<EarthquakeInfoViewModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<EarthquakeInfoViewModel>>(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEarthquakeInfo([FromServices] IEarthquakeApp app)
        {
            var response = await app.GetEarthquakeData();

            if (!response.Any())
                return NoContent();

            return Ok(response);
        }
    }
}
