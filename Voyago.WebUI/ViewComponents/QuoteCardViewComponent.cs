using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class QuoteCardViewComponent : ViewComponent
{
    private readonly IVoyagoApiClient _apiClient;

    public QuoteCardViewComponent(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var quotes = await _apiClient.GetRandomQuotesAsync(count: 2);
        return View(quotes);
    }
}