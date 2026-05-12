using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;

public class RapidNewsResponseDto
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("data")]
    public List<RapidNewsArticleDto>? Data { get; set; }
}

public class RapidNewsArticleDto
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("snippet")]
    public string? Snippet { get; set; }

    [JsonPropertyName("photo_url")]
    public string? PhotoUrl { get; set; }

    [JsonPropertyName("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [JsonPropertyName("published_datetime_utc")]
    public DateTime? PublishedDatetimeUtc { get; set; }

    [JsonPropertyName("source_name")]
    public string? SourceName { get; set; }

    [JsonPropertyName("source_logo_url")]
    public string? SourceLogoUrl { get; set; }


}