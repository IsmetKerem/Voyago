using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;



public class RapidDestinationResponseDto
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("data")]
    public List<RapidDestinationDto>? Data { get; set; }
}

public class RapidDestinationDto
{
    [JsonPropertyName("dest_id")]
    public string? DestId { get; set; }

    [JsonPropertyName("search_type")]
    public string? SearchType { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("city_name")]
    public string? CityName { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("nr_hotels")]
    public int NrHotels { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("dest_type")]
    public string? DestType { get; set; }
}


public class RapidHotelsSearchResponseDto
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("data")]
    public RapidHotelsSearchDataDto? Data { get; set; }
}

public class RapidHotelsSearchDataDto
{
    [JsonPropertyName("hotels")]
    public List<RapidHotelDto>? Hotels { get; set; }

    [JsonPropertyName("meta")]
    public List<RapidMetaDto>? Meta { get; set; }
}

public class RapidMetaDto
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

public class RapidHotelDto
{
    [JsonPropertyName("hotel_id")]
    public long HotelId { get; set; }

    [JsonPropertyName("accessibilityLabel")]
    public string? AccessibilityLabel { get; set; }

    [JsonPropertyName("property")]
    public RapidPropertyDto? Property { get; set; }
}

public class RapidPropertyDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("wishlistName")]
    public string? WishlistName { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("propertyClass")]
    public int PropertyClass { get; set; }

    [JsonPropertyName("accuratePropertyClass")]
    public int AccuratePropertyClass { get; set; }

    [JsonPropertyName("reviewScore")]
    public double ReviewScore { get; set; }

    [JsonPropertyName("reviewScoreWord")]
    public string? ReviewScoreWord { get; set; }

    [JsonPropertyName("reviewCount")]
    public int ReviewCount { get; set; }

    [JsonPropertyName("isPreferred")]
    public bool IsPreferred { get; set; }

    [JsonPropertyName("photoUrls")]
    public List<string>? PhotoUrls { get; set; }

    [JsonPropertyName("mainPhotoId")]
    public long MainPhotoId { get; set; }

    [JsonPropertyName("checkinDate")]
    public string? CheckinDate { get; set; }

    [JsonPropertyName("checkoutDate")]
    public string? CheckoutDate { get; set; }

    [JsonPropertyName("priceBreakdown")]
    public RapidPriceBreakdownDto? PriceBreakdown { get; set; }
}

public class RapidPriceBreakdownDto
{
    [JsonPropertyName("grossPrice")]
    public RapidPriceDto? GrossPrice { get; set; }

    [JsonPropertyName("strikethroughPrice")]
    public RapidPriceDto? StrikethroughPrice { get; set; }

    [JsonPropertyName("excludedPrice")]
    public RapidPriceDto? ExcludedPrice { get; set; }
}

public class RapidPriceDto
{
    [JsonPropertyName("value")]
    public double Value { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}