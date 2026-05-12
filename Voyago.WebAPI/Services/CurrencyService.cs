using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    
    private static readonly List<(string Code, string Name, string Symbol)> SupportedCurrencies = new()
    {
        ("EUR", "Euro", "€"),
        ("GBP", "British Pound", "£"),
        ("JPY", "Japanese Yen", "¥"),
        ("TRY", "Turkish Lira", "₺"),
        ("CAD", "Canadian Dollar", "C$"),
        ("AUD", "Australian Dollar", "A$")
    };
    
    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CurrencyDto?> GetLatestRatesAsync()
    {
        
        var symbols = string.Join(",", SupportedCurrencies.Select(c => c.Code));
        var url = $"latest?base=USD&symbols={symbols}";
        
        var response = await _httpClient.GetFromJsonAsync<RapidCurrencyResponseDto>(url);
        
        if (response is null || !response.Success || response.Rates is null)
        {
            return null;
        }
        
        var rates = SupportedCurrencies
            .Where(c => response.Rates.ContainsKey(c.Code))
            .Select(c => new CurrencyRate
            {
                Code = c.Code,
                Name = c.Name,
                Symbol = c.Symbol,
                Rate = response.Rates[c.Code]
            })
            .ToList();
        
        return new CurrencyDto
        {
            BaseCurrency = response.Base ?? "USD",
            Date = response.Date ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
            Rates = rates
        };
    }
}