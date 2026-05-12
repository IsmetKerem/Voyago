namespace Voyago.WebAPI.Dtos;

public class CryptoDto
{
    public List<CryptoCoinDto> Coins { get; set; } = new();
}

public class CryptoCoinDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal ChangePercent { get; set; }
    public int Rank { get; set; }
}