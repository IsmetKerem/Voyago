using System.Text.Json;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class QuoteService : IQuoteService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<QuoteService> _logger;

    public QuoteService(HttpClient httpClient, ILogger<QuoteService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<QuoteDto>> GetRandomQuotesAsync(int count = 2)
    {
        try
        {
            var response = await _httpClient.GetAsync("get_quotes.php?limit=10&page=1");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<RapidQuoteResponseDto>(json);

            if (data?.Quotes is null || data.Quotes.Count == 0)
            {
                _logger.LogWarning("Quotes API returned empty list");
                return new List<QuoteDto>();
            }

            var validQuotes = data.Quotes
                .Where(q => !string.IsNullOrWhiteSpace(q.Quote)
                            && !string.IsNullOrWhiteSpace(q.Author))
                .ToList();

            if (validQuotes.Count == 0)
            {
                return new List<QuoteDto>();
            }

            var pickCount = Math.Min(count, validQuotes.Count);

         
            var selected = validQuotes
                .OrderBy(_ => Random.Shared.Next())
                .Take(pickCount)
                .Select(MapToDto)
                .ToList();

            return selected;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Quote API request failed");
            return new List<QuoteDto>();
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Quote API JSON deserialization failed");
            return new List<QuoteDto>();
        }
    }

    private static QuoteDto MapToDto(RapidQuoteItemDto item) => new()
    {
        Text = item.Quote ?? string.Empty,
        Author = item.Author ?? "Unknown",
        Category = item.Category ?? string.Empty
    };
}