using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IAirQualityService
{
    Task<AirQualityDto?> GetAirQualityAsync(string city);
}