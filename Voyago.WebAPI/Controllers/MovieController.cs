using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomMovie()
    {
        var movie = await _movieService.GetRandomTopMovieAsync();

        if (movie is null)
        {
            return NotFound();
        }

        return Ok(movie);
    }
}