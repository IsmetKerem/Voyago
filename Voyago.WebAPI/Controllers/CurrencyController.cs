using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var rates = await _currencyService.GetLatestRatesAsync();

        if (rates is null)
        {
            return NotFound("Currency data unavailable");
        }

        return Ok(rates);
    }
}