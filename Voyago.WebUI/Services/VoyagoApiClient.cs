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
    public async Task<List<NewsDto>> GetTopHeadlinesAsync(int limit = 5)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/news/top-headlines?limit={limit}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[VoyagoApiClient] News API returned status {response.StatusCode}");
                return new List<NewsDto>();
            }

            var news = await response.Content.ReadFromJsonAsync<List<NewsDto>>();
            return news ?? new List<NewsDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] News error: {ex.Message}");
            return new List<NewsDto>();
        }
    }
    public async Task<MovieDto?> GetRandomMovieAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MovieDto>("api/movie/random");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Movie error: {ex.Message}");
            return null;
        }
    }
    public async Task<List<QuoteDto>> GetRandomQuotesAsync(int count = 2)
    {
        try
        {
            var result = await _httpClient
                .GetFromJsonAsync<List<QuoteDto>>($"api/quote/random?count={count}");

            return result ?? new List<QuoteDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Quote error: {ex.Message}");
            return new List<QuoteDto>();
        }
    }
    public async Task<AirQualityDto?> GetAirQualityAsync(string city)
    {
        try
        {
            var encodedCity = Uri.EscapeDataString(city);
            return await _httpClient
                .GetFromJsonAsync<AirQualityDto>($"api/airquality?city={encodedCity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] AirQuality error: {ex.Message}");
            return null;
        }
    }
    
    
}