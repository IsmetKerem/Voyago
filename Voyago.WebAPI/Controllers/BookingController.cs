using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Dtos;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchHotels([FromQuery] HotelSearchRequest request)
    {
        var result = await _bookingService.SearchHotelsAsync(request);

        if (result is null)
        {
            return BadRequest("Search failed. Please try again.");
        }

        return Ok(result);
    }
}