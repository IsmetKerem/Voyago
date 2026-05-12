using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class CurrencyCardViewComponent : ViewComponent
{
    private readonly IVoyagoApiClient _apiClient;

    public CurrencyCardViewComponent(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var currency = await _apiClient.GetCurrencyAsync();
        return View(currency);
    }
}