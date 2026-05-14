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
}