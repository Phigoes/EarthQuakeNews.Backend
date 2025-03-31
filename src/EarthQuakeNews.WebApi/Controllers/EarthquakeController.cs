using EarthQuakeNews.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuakeNews.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EarthquakeController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEarthquakeInfo([FromServices] IEarthquakeApp app)
        {
            var response = await app.GetEarthquakeData();

            return Ok(response);
        }
    }
}
