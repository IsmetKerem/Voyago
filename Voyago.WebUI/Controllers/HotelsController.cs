using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Dtos;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.Controllers;

public class HotelsController : Controller
{
    private readonly IVoyagoApiClient _apiClient;

    public HotelsController(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index([FromQuery] HotelSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Destination))
        {
            request.Destination = "Paris";
        }

        if (string.IsNullOrWhiteSpace(request.ArrivalDate))
        {
            request.ArrivalDate = DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd");
        }

        if (string.IsNullOrWhiteSpace(request.DepartureDate))
        {
            request.DepartureDate = DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd");
        }

        var response = await _apiClient.SearchHotelsAsync(request);

        ViewBag.SearchRequest = request;

        return View(response);
    }
    [Route("Hotels/Detail/{hotelId:long}")]
    public async Task<IActionResult> Detail(
        long hotelId,
        [FromQuery] string? arrivalDate,
        [FromQuery] string? departureDate,
        [FromQuery] int adults = 2,
        [FromQuery] int rooms = 1)
    {
        var hotel = await _apiClient.GetHotelDetailAsync(
            hotelId, arrivalDate, departureDate, adults, rooms);

        if (hotel is null)
        {
            TempData["Error"] = "Sorry, we couldn't load this hotel's details. It may be temporarily unavailable.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.ArrivalDate = arrivalDate;
        ViewBag.DepartureDate = departureDate;
        ViewBag.Adults = adults;
        ViewBag.Rooms = rooms;

        return View(hotel);
    }
}