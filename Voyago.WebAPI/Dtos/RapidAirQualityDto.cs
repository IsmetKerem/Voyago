using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;


public class PollutantDto
{
    [JsonPropertyName("concentration")]
    public double Concentration { get; set; }

    [JsonPropertyName("aqi")]
    public int Aqi { get; set; }
}

public class RapidAirQualityDto
{
    [JsonPropertyName("CO")]
    public PollutantDto? CO { get; set; }

    [JsonPropertyName("NO2")]
    public PollutantDto? NO2 { get; set; }

    [JsonPropertyName("O3")]
    public PollutantDto? O3 { get; set; }

    [JsonPropertyName("SO2")]
    public PollutantDto? SO2 { get; set; }


    [JsonPropertyName("PM2.5")]
    public PollutantDto? Pm25 { get; set; }

    [JsonPropertyName("PM10")]
    public PollutantDto? Pm10 { get; set; }

    [JsonPropertyName("overall_aqi")]
    public int OverallAqi { get; set; }
}