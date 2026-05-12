using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class CryptoCardViewComponent:ViewComponent
{
    private readonly IVoyagoApiClient _voyagoApiClient;

    public CryptoCardViewComponent(IVoyagoApiClient voyagoApiClient)
    {
        _voyagoApiClient = voyagoApiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var values = await _voyagoApiClient.GetCryptoAsync();
        return View(values);
    }
}