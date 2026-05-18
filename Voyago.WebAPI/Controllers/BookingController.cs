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
    [HttpGet("hotel/{hotelId:long}")]
    public async Task<IActionResult> GetHotelDetail(
        long hotelId,
        [FromQuery] string? arrivalDate,
        [FromQuery] string? departureDate,
        [FromQuery] int adults = 2,
        [FromQuery] int rooms = 1)
    {
        var result = await _bookingService.GetHotelDetailAsync(
            hotelId, arrivalDate, departureDate, adults, rooms);

        if (result is null)
        {
            return NotFound(new { message = "Hotel not found." });
        }

        return Ok(result);
    }
}