using Voyago.WebUI.Dtos;

namespace Voyago.WebUI.Services;

public class VoyagoApiClient : IVoyagoApiClient
{
    private readonly HttpClient _httpClient;

    public VoyagoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherDto?> GetWeatherAsync(string city)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<WeatherDto>(
                $"api/weather?city={Uri.EscapeDataString(city)}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}