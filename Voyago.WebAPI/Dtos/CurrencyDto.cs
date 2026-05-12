namespace Voyago.WebAPI.Dtos;

public class CurrencyDto
{
    public string BaseCurrency { get; set; } = "USD";
    public string Date { get; set; } = string.Empty;
    public List<CurrencyRate> Rates { get; set; } = new();
}

public class CurrencyRate
{
    public string Code { get; set; } = string.Empty;        // "EUR"
    public string Name { get; set; } = string.Empty;        // "Euro"
    public string Symbol { get; set; } = string.Empty;      // "€"
    public decimal Rate { get; set; }                       // 0.85
}