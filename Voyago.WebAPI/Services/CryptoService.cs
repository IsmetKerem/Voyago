using System.Globalization;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class CryptoService:ICryptoService
{
    private readonly HttpClient _httpClient;

    public CryptoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private const int TopCount = 6; 
    public async Task<CryptoDto?> GetTopCoinsAsync()
    {
        var url = $"coins?limit={TopCount}&offset=0";
        
        var response = await _httpClient.GetFromJsonAsync<RapidCryptoResponseDto>(url);
        
        if (response?.Data?.Coins is null || response.Status != "success")
        {
            return null;
        }
        
        var coins = response.Data.Coins
            .Select(c => new CryptoCoinDto
            {
                Symbol = c.Symbol ?? string.Empty,
                Name = c.Name ?? string.Empty,
                IconUrl = c.IconUrl ?? string.Empty,
                Price = ParseDecimal(c.Price),
                ChangePercent = ParseDecimal(c.Change),
                Rank = c.Rank
            })
            .ToList();
        
        return new CryptoDto { Coins = coins };
    }
    
    private static decimal ParseDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0m;
        
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) 
            ? result 
            : 0m;

    }
}