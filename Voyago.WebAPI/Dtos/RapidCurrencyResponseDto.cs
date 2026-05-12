namespace Voyago.WebAPI.Dtos;

public class RapidCurrencyResponseDto
{
    public bool Success { get; set; }
    public long Timestamp { get; set; }
    public string? Base { get; set; }
    public string? Date { get; set; }
    public Dictionary<string, decimal>? Rates { get; set; }
}