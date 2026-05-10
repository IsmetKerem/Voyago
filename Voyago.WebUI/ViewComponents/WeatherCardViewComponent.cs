using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class WeatherCardViewComponent : ViewComponent
{
    private readonly IVoyagoApiClient _apiClient;

    public WeatherCardViewComponent(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync(string city = "Paris")
    {
        var weather = await _apiClient.GetWeatherAsync(city);
        return View(weather);
    }
}