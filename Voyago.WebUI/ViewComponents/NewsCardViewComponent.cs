using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class NewsCardViewComponent : ViewComponent
{
    private readonly IVoyagoApiClient _apiClient;

    public NewsCardViewComponent(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var news = await _apiClient.GetTopHeadlinesAsync(limit: 4);
        return View(news);
    }
}