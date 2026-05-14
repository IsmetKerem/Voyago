using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirQualityController : ControllerBase
{
    private readonly IAirQualityService _airQualityService;

    public AirQualityController(IAirQualityService airQualityService)
    {
        _airQualityService = airQualityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAirQuality([FromQuery] string city = "Paris")
    {
        var result = await _airQualityService.GetAirQualityAsync(city);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}