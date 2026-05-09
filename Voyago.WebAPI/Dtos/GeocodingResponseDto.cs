namespace Voyago.WebAPI.Dtos;

public class GeocodingResponseDto
{
    public List<GeocodingResult>? Results { get; set; }
}

public class GeocodingResult
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}