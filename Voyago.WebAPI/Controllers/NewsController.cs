using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet("top-headlines")]
    public async Task<IActionResult> GetTopHeadlines([FromQuery] int limit = 5)
    {
        var news = await _newsService.GetTopHeadlinesAsync(limit);
        return Ok(news);
    }
}