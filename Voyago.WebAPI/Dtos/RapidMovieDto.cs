using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;

public class RapidMovieDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("primaryTitle")]
    public string? PrimaryTitle { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("primaryImage")]
    public string? PrimaryImage { get; set; }

    [JsonPropertyName("trailer")]
    public string? Trailer { get; set; }

    [JsonPropertyName("startYear")]
    public int? StartYear { get; set; }

    [JsonPropertyName("runtimeMinutes")]
    public int? RuntimeMinutes { get; set; }

    [JsonPropertyName("averageRating")]
    public double? AverageRating { get; set; }

    [JsonPropertyName("genres")]
    public List<string>? Genres { get; set; }

    [JsonPropertyName("contentRating")]
    public string? ContentRating { get; set; }


}