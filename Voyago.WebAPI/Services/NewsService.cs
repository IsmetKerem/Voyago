using System.Text.Json;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class NewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NewsService> _logger;

    public NewsService(HttpClient httpClient, ILogger<NewsService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<NewsDto>> GetTopHeadlinesAsync(int limit = 5)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                "top-headlines?limit=10&country=US&lang=en");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var rapidResponse = JsonSerializer.Deserialize<RapidNewsResponseDto>(json);

            if (rapidResponse?.Data is null || rapidResponse.Data.Count == 0)
            {
                _logger.LogWarning("News API returned empty data");
                return new List<NewsDto>();
            }

            var newsList = rapidResponse.Data
                .Where(a => !string.IsNullOrWhiteSpace(a.Title)
                         && !string.IsNullOrWhiteSpace(a.Link))
                .Take(limit)
                .Select(a => new NewsDto
                {
                    Title = a.Title!,
                    Link = a.Link!,
                    Snippet = a.Snippet,
                    PhotoUrl = a.PhotoUrl,
                    ThumbnailUrl = a.ThumbnailUrl,
                    SourceName = a.SourceName ?? "Unknown",
                    SourceLogoUrl = a.SourceLogoUrl,
                    PublishedAtUtc = a.PublishedDatetimeUtc ?? DateTime.UtcNow
                })
                .ToList();

            return newsList;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "News API HTTP request failed");
            return new List<NewsDto>();
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "News API JSON deserialization failed");
            return new List<NewsDto>();
        }
    }
}