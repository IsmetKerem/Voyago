namespace Voyago.WebAPI.Dtos;

public class HotelDetailDto
{
    public long HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public int StarRating { get; set; }
    public double ReviewScore { get; set; }
    public string ReviewScoreWord { get; set; } = string.Empty;
    public int ReviewCount { get; set; }

    public List<string> PhotoUrls { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public List<string> Facilities { get; set; } = new();

    public decimal? PricePerNight { get; set; }
    public decimal? TotalPrice { get; set; }
    public string Currency { get; set; } = "USD";

    public string BookingUrl { get; set; } = string.Empty;

    // ===== Helper: Google Maps URL =====
    public string GoogleMapsUrl =>
        $"https://www.google.com/maps?q={Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

    // ===== Helper: Review score class for color =====
    public string ReviewScoreClass => ReviewScore switch
    {
        >= 9.0 => "review-excellent",
        >= 8.0 => "review-good",
        >= 7.0 => "review-okay",
        _ => "review-low"
    };

    // ===== Helper: Review score formatted =====
    public string ReviewScoreFormatted =>
        ReviewScore.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

    // ===== Helper: Currency symbol =====
    public string CurrencySymbol => Currency switch
    {
        "USD" => "$",
        "EUR" => "€",
        "GBP" => "£",
        "TRY" => "₺",
        _ => Currency + " "
    };
}