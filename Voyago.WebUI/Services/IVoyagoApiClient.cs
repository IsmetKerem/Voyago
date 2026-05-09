using Voyago.WebUI.Dtos;

namespace Voyago.WebUI.Services;

public interface IVoyagoApiClient
{
    Task<WeatherDto?> GetWeatherAsync(string city);
}