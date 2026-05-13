using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.ViewComponents;

public class MovieCardViewComponent : ViewComponent
{
    private readonly IVoyagoApiClient _apiClient;

    public MovieCardViewComponent(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var movie = await _apiClient.GetRandomMovieAsync();
        return View(movie);
    }
}