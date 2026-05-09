using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;

public class OpenMeteoResponseDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    [JsonPropertyName("current_weather")]
    public CurrentWeather? CurrentWeather { get; set; }
}

public class CurrentWeather
{
    public double Temperature { get; set; }

    [JsonPropertyName("weathercode")]
    public int WeatherCode { get; set; }

    public string? Time { get; set; }
    public double Windspeed { get; set; }
    public double Winddirection { get; set; }
}