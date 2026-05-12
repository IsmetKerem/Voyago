namespace Voyago.WebAPI.Dtos;

public class RapidCryptoResponseDto
{
    public string? Status { get; set; }
    public CryptoData? Data { get; set; }
}

public class CryptoData
{
    public List<CryptoCoin>? Coins { get; set; }
}

public class CryptoCoin
{
    public string? Uuid { get; set; }
    public string? Symbol { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public string? IconUrl { get; set; }
    public string? Price { get; set; }       
    public string? Change { get; set; }      
    public string? MarketCap { get; set; }
    public int Rank { get; set; }
}