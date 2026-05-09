using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IWeatherService
{
    Task<WeatherDto?> GetWeatherAsync(string city);
}