using Voyago.WebUI.Dtos;
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
    public async Task<CurrencyDto?> GetCurrencyAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CurrencyDto>("api/currency");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Currency error: {ex.Message}");
            return null;
        }
    }

    public async Task<CryptoDto?> GetCryptoAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CryptoDto>("api/Crypto");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Crypto error: {ex.Message}");
            return null;
           
        }
    }
}