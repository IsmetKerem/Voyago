using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class AirQualityCardViewComponent : ViewComponent
{
    private readonly IVoyagoApiClient _apiClient;

    public AirQualityCardViewComponent(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync(string city = "Paris")
    {
        var airQuality = await _apiClient.GetAirQualityAsync(city);
        return View(airQuality);
    }
}