using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FootballController : ControllerBase
{
    private readonly IFootballService _footballService;

    public FootballController(IFootballService footballService)
    {
        _footballService = footballService;
    }

    [HttpGet("top-matches")]
    public async Task<IActionResult> GetTopMatches([FromQuery] int count = 4)
    {
        var matches = await _footballService.GetTopMatchesAsync(count);
        return Ok(matches);
    }
}