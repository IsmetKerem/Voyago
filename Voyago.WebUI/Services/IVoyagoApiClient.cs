using Voyago.WebUI.Dtos;
using Voyago.WebUI.Dtos;

namespace Voyago.WebUI.Services;

public interface IVoyagoApiClient
{
    Task<WeatherDto?> GetWeatherAsync(string city);
    Task<CurrencyDto?> GetCurrencyAsync();
    Task<CryptoDto?> GetCryptoAsync();
    Task<List<NewsDto>> GetTopHeadlinesAsync(int limit = 5);
    Task<MovieDto?> GetRandomMovieAsync();
    Task<List<QuoteDto>> GetRandomQuotesAsync(int count = 2);
    Task<AirQualityDto?> GetAirQualityAsync(string city);
    Task<List<FootballMatchDto>> GetTopMatchesAsync(int count = 4);
}