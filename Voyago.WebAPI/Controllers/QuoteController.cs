using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuoteController : ControllerBase
{
    private readonly IQuoteService _quoteService;

    public QuoteController(IQuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomQuotes([FromQuery] int count = 2)
    {
        var quotes = await _quoteService.GetRandomQuotesAsync(count);
        return Ok(quotes);
    }
}