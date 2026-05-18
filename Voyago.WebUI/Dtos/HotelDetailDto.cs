namespace Voyago.WebUI.Dtos;

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
    public string GoogleMapsUrl { get; set; } = string.Empty;
    public string ReviewScoreClass { get; set; } = string.Empty;
    public string ReviewScoreFormatted { get; set; } = string.Empty;
    public string CurrencySymbol { get; set; } = string.Empty;


    public string GetHighResPhoto(int index)
    {
        if (index >= PhotoUrls.Count) return string.Empty;
        return PhotoUrls[index].Replace("/square60/", "/max1024x768/");
    }

    public string GetMainPhoto() =>
        PhotoUrls.Count > 0 ? GetHighResPhoto(0) : string.Empty;

    public bool HasPrice => PricePerNight.HasValue && PricePerNight.Value > 0;

    public int? PricePerNightRounded => HasPrice ? (int)Math.Round(PricePerNight!.Value) : null;
    public int? TotalPriceRounded => TotalPrice.HasValue ? (int)Math.Round(TotalPrice.Value) : null;

    public string FullAddress
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Address)) parts.Add(Address);
            if (!string.IsNullOrWhiteSpace(City)) parts.Add(City);
            if (!string.IsNullOrWhiteSpace(Zip)) parts.Add(Zip);
            if (!string.IsNullOrWhiteSpace(Country)) parts.Add(Country);
            return string.Join(", ", parts);
        }
    }
}