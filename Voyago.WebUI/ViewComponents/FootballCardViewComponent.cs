using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class FootballCardViewComponent : ViewComponent
{
    private readonly IVoyagoApiClient _apiClient;

    public FootballCardViewComponent(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var matches = await _apiClient.GetTopMatchesAsync(count: 4);
        return View(matches);
    }
}