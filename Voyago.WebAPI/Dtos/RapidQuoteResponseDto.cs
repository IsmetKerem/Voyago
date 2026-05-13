using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;

public class RapidQuoteResponseDto
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("total_quotes")]
    public long TotalQuotes { get; set; }

    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    [JsonPropertyName("quotes")]
    public List<RapidQuoteItemDto>? Quotes { get; set; }
}

public class RapidQuoteItemDto
{
    [JsonPropertyName("quote_id")]
    public string? QuoteId { get; set; }

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("quote")]
    public string? Quote { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }
}