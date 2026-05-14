using System.Text.Json;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class AirQualityService : IAirQualityService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AirQualityService> _logger;

    public AirQualityService(HttpClient httpClient, ILogger<AirQualityService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<AirQualityDto?> GetAirQualityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            _logger.LogWarning("AirQuality requested with empty city");
            return null;
        }

        try
        {
            var encodedCity = Uri.EscapeDataString(city);
            var response = await _httpClient.GetAsync($"v1/airquality?city={encodedCity}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<RapidAirQualityDto>(json);

            if (data is null)
            {
                _logger.LogWarning("AirQuality API returned null for {City}", city);
                return null;
            }

            return MapToDto(data, city);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "AirQuality API request failed for {City}", city);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "AirQuality API JSON deserialization failed");
            return null;
        }
    }

    private static AirQualityDto MapToDto(RapidAirQualityDto rapid, string city)
    {
        return new AirQualityDto
        {
            City = city,
            OverallAqi = rapid.OverallAqi,
            Pm25Concentration = rapid.Pm25?.Concentration ?? 0,
            Pm10Concentration = rapid.Pm10?.Concentration ?? 0,
            O3Concentration = rapid.O3?.Concentration ?? 0,
            No2Concentration = rapid.NO2?.Concentration ?? 0,
            SO2Concentration = rapid.SO2?.Concentration ?? 0,
            COConcentration = rapid.CO?.Concentration ?? 0
        };
    }
}