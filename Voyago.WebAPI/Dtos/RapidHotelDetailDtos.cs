using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;

// =====================================================
// Top-level response
// =====================================================
public class RapidHotelDetailResponseDto
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("data")]
    public RapidHotelDetailDataDto? Data { get; set; }
}

// =====================================================
// Main data block — burada bilgilerin çoğu var
// =====================================================
public class RapidHotelDetailDataDto
{
    [JsonPropertyName("hotel_id")]
    public long HotelId { get; set; }

    [JsonPropertyName("hotel_name")]
    public string? HotelName { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("city_trans")]
    public string? CityTranslated { get; set; }

    [JsonPropertyName("country_trans")]
    public string? CountryTranslated { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("review_score")]
    public double ReviewScore { get; set; }

    [JsonPropertyName("review_score_word")]
    public string? ReviewScoreWord { get; set; }

    [JsonPropertyName("review_nr")]
    public int ReviewCount { get; set; }

    [JsonPropertyName("accuratePropertyClass")]
    public int AccuratePropertyClass { get; set; }

    [JsonPropertyName("propertyClass")]
    public int PropertyClass { get; set; }

    [JsonPropertyName("facilities_block")]
    public RapidFacilitiesBlockDto? FacilitiesBlock { get; set; }

    [JsonPropertyName("rawData")]
    public RapidHotelRawDataDto? RawData { get; set; }

    // NOT: composite_price_breakdown getHotelDetails endpoint'inde de aynı yapıda
    // ama search endpoint'inde "priceBreakdown" key'iyle gelir.
    // İki ayrı class isteyebilir, kontrol için iki olası key'i deneyeceğiz.
    [JsonPropertyName("composite_price_breakdown")]
    public RapidCompositePriceBreakdownDto? CompositePriceBreakdown { get; set; }
}

// =====================================================
// Facilities (Wi-Fi, parking, pool...)
// =====================================================
public class RapidFacilitiesBlockDto
{
    [JsonPropertyName("facilities")]
    public List<RapidFacilityDto>? Facilities { get; set; }
}

public class RapidFacilityDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}

// =====================================================
// Raw data — photo URLs + description
// =====================================================
public class RapidHotelRawDataDto
{
    [JsonPropertyName("photoUrls")]
    public List<string>? PhotoUrls { get; set; }

    [JsonPropertyName("description_translations")]
    public List<RapidDescriptionDto>? DescriptionTranslations { get; set; }
}

public class RapidDescriptionDto
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

// =====================================================
// Composite price breakdown (detail endpoint'e özel)
// Search'teki RapidPriceBreakdownDto'dan farklı çünkü buradaki yapı:
// - gross_amount_per_night
// - gross_amount (total)
// Search'teki ise:
// - grossPrice
// - strikethroughPrice
// =====================================================
public class RapidCompositePriceBreakdownDto
{
    [JsonPropertyName("gross_amount_per_night")]
    public RapidGrossAmountDto? GrossAmountPerNight { get; set; }

    [JsonPropertyName("gross_amount")]
    public RapidGrossAmountDto? GrossAmountTotal { get; set; }
}

public class RapidGrossAmountDto
{
    [JsonPropertyName("value")]
    public double Value { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}