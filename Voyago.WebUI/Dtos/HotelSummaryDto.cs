namespace Voyago.WebUI.Dtos;

public class HotelSummaryDto
{
    public long HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PhotoUrl { get; set; }
    public int StarRating { get; set; }
    public double ReviewScore { get; set; }
    public string ReviewScoreWord { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string Currency { get; set; } = "USD";
    public bool IsPreferred { get; set; }

    public int? DiscountPercent
    {
        get
        {
            if (OriginalPrice is null || OriginalPrice <= Price) return null;
            var pct = (int)Math.Round((OriginalPrice.Value - Price) / OriginalPrice.Value * 100);
            return pct >= 5 ? pct : null;
        }
    }

    public string ReviewScoreFormatted =>
        ReviewScore.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

    public int PriceRounded => (int)Math.Round(Price);

    public int? OriginalPriceRounded =>
        OriginalPrice.HasValue ? (int)Math.Round(OriginalPrice.Value) : null;

 
    public string ReviewScoreClass => ReviewScore switch
    {
        >= 9 => "review-excellent",
        >= 8 => "review-good",
        >= 7 => "review-okay",
        _ => "review-low"
    };

    public string CurrencySymbol => Currency switch
    {
        "USD" => "$",
        "EUR" => "€",
        "GBP" => "£",
        _ => Currency 
    };
    // Booking.com search URL — kullanıcıyı oteli aratılmış olarak götürür
    public string BuildBookingUrl(string? arrivalDate, string? departureDate, int adults, int rooms)
    {
        var query = new List<string>
        {
            $"ss={Uri.EscapeDataString(Name)}"
        };

        if (!string.IsNullOrWhiteSpace(arrivalDate))
            query.Add($"checkin={Uri.EscapeDataString(arrivalDate)}");

        if (!string.IsNullOrWhiteSpace(departureDate))
            query.Add($"checkout={Uri.EscapeDataString(departureDate)}");

        query.Add($"group_adults={adults}");
        query.Add($"no_rooms={rooms}");

        return $"https://www.booking.com/searchresults.html?{string.Join("&", query)}";
    }
}