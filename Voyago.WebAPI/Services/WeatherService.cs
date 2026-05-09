using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherDto?> GetWeatherAsync(string city)
    {
        var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=en&format=json";
        var geoResponse = await _httpClient.GetFromJsonAsync<GeocodingResponseDto>(geoUrl);

        var firstResult = geoResponse?.Results?.FirstOrDefault();
        if (firstResult is null)
        {
            return null;
        }

        var weatherUrl = $"https://api.open-meteo.com/v1/forecast" +
                         $"?latitude={firstResult.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&longitude={firstResult.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                         $"&current_weather=true";

        var weatherResponse = await _httpClient.GetFromJsonAsync<OpenMeteoResponseDto>(weatherUrl);

        if (weatherResponse?.CurrentWeather is null)
        {
            return null;
        }

        // 3. ADIM: Map et
        return new WeatherDto
        {
            City = firstResult.Name ?? city,
            Country = firstResult.Country ?? string.Empty,
            TemperatureCelsius = weatherResponse.CurrentWeather.Temperature,
            Description = MapWeatherCodeToDescription(weatherResponse.CurrentWeather.WeatherCode),
            Icon = MapWeatherCodeToIcon(weatherResponse.CurrentWeather.WeatherCode)
        };
    }

    private static string MapWeatherCodeToDescription(int code) => code switch
    {
        0 => "Clear sky",
        1 or 2 or 3 => "Partly cloudy",
        45 or 48 => "Foggy",
        51 or 53 or 55 => "Drizzle",
        61 or 63 or 65 => "Rainy",
        71 or 73 or 75 => "Snowy",
        80 or 81 or 82 => "Rain showers",
        95 or 96 or 99 => "Thunderstorm",
        _ => "Unknown"
    };

    private static string MapWeatherCodeToIcon(int code) => code switch
    {
        0 => "☀️",
        1 or 2 or 3 => "⛅",
        45 or 48 => "🌫️",
        51 or 53 or 55 => "🌦️",
        61 or 63 or 65 => "🌧️",
        71 or 73 or 75 => "❄️",
        80 or 81 or 82 => "🌧️",
        95 or 96 or 99 => "⛈️",
        _ => "❓"
    };
}