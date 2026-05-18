using System.Globalization;
using System.Text.Json;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class BookingService : IBookingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BookingService> _logger;

    public BookingService(HttpClient httpClient, ILogger<BookingService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<HotelSearchResponseDto?> SearchHotelsAsync(HotelSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Destination))
        {
            _logger.LogWarning("Booking search with empty destination");
            return null;
        }

        try
        {
            var destination = await ResolveDestinationAsync(request.Destination);

            if (destination is null)
            {
                _logger.LogWarning("Destination not found: {Dest}", request.Destination);
                return new HotelSearchResponseDto
                {
                    DestinationName = request.Destination,
                    TotalCount = 0
                };
            }

            var hotelsResponse = await FetchHotelsAsync(destination, request);

            return hotelsResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Booking search failed for {Dest}", request.Destination);
            return null;
        }
    }

    private async Task<RapidDestinationDto?> ResolveDestinationAsync(string query)
    {
        var encodedQuery = Uri.EscapeDataString(query);
        var url = $"api/v1/hotels/searchDestination?query={encodedQuery}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<RapidDestinationResponseDto>(json);

        if (data?.Data is null || data.Data.Count == 0)
            return null;

        var preferred = data.Data
            .FirstOrDefault(d => d.DestType == "city")
            ?? data.Data.First();

        _logger.LogInformation("Destination resolved: {Name} ({DestId})",
            preferred.Name, preferred.DestId);

        return preferred;
    }

    private async Task<HotelSearchResponseDto> FetchHotelsAsync(
        RapidDestinationDto destination,
        HotelSearchRequest request)
    {
        // ---- Date validation ----
        if (!DateOnly.TryParse(request.ArrivalDate, CultureInfo.InvariantCulture, out var arrival))
        {
            arrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7));
            _logger.LogWarning("Invalid arrival date '{Date}', using default", request.ArrivalDate);
        }

        if (!DateOnly.TryParse(request.DepartureDate, CultureInfo.InvariantCulture, out var departure))
        {
            departure = arrival.AddDays(3);
            _logger.LogWarning("Invalid departure date '{Date}', using default", request.DepartureDate);
        }

        if (departure <= arrival)
        {
            departure = arrival.AddDays(1);
        }

        var arrivalStr = arrival.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var departureStr = departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var searchType = destination.SearchType?.ToUpperInvariant() ?? "CITY";

        // ---- Base query parameters ----
        var queryParams = new Dictionary<string, string>
        {
            ["dest_id"] = destination.DestId ?? string.Empty,
            ["search_type"] = searchType,
            ["arrival_date"] = arrivalStr,
            ["departure_date"] = departureStr,
            ["adults"] = request.Adults.ToString(CultureInfo.InvariantCulture),
            ["room_qty"] = request.Rooms.ToString(CultureInfo.InvariantCulture),
            ["page_number"] = request.PageNumber.ToString(CultureInfo.InvariantCulture),
            ["units"] = "metric",
            ["temperature_unit"] = "c",
            ["languagecode"] = "en-us",
            ["currency_code"] = "USD"
        };

        if (request.Children > 0)
        {
            var childAges = string.Join(",", Enumerable.Repeat("7", request.Children));
            queryParams["children_age"] = childAges;
        }

        // ============================================================
        // OPTIONAL FILTERS — Booking-com15 specific
        // ============================================================
        // Booking-com15 filter sistemi tek "categories_filter" parametresi alır,
        // içinde "type::value" formatında multi-value string.
        // Örnek: categories_filter=price::USD-100-500,class::5,class::4
        // ============================================================

        var categoryFilters = new List<string>();

        // PRICE filter — Format: price::USD-{min}-{max}
        // Booking min/max ikisini de bekler. Tek taraf verilmişse default kullan.
        if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
        {
            var min = request.MinPrice?.ToString("0", CultureInfo.InvariantCulture) ?? "1";
            var max = request.MaxPrice?.ToString("0", CultureInfo.InvariantCulture) ?? "99999";
            categoryFilters.Add($"price::USD-{min}-{max}");
        }

        // STAR RATING filter — her yıldız ayrı entry
        // Format: class::5, class::4 (ayrı entries)
        if (request.StarRatings is { Count: > 0 })
        {
            foreach (var star in request.StarRatings)
            {
                categoryFilters.Add($"class::{star}");
            }
        }

        // Tüm category filter'lar tek parametrede virgülle ayrılır
        if (categoryFilters.Count > 0)
        {
            queryParams["categories_filter"] = string.Join(",", categoryFilters);
        }

        // REVIEW SCORE — ayrı parametre
        // 70 = 7+, 80 = 8+, 90 = 9+
        if (request.MinReviewScore.HasValue)
        {
            queryParams["review_score"] = request.MinReviewScore.Value.ToString();
        }

        // ---- Build URL ----
        var queryString = string.Join("&", queryParams
            .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));

        var url = $"api/v1/hotels/searchHotels?{queryString}";

        // DEBUG log — Booking'e giden tam URL
        _logger.LogInformation("Booking URL: {Url}", url);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<RapidHotelsSearchResponseDto>(json);

        if (data?.Data?.Hotels is null)
        {
            return new HotelSearchResponseDto
            {
                DestinationName = destination.Name ?? destination.CityName ?? "Unknown",
                TotalCount = 0
            };
        }

        var totalCount = ParseTotalCount(data.Data.Meta);

        // ============================================================
        // CLIENT-SIDE FILTER VALIDATION
        // Booking API filter'ları bazen yanlış yorumluyor.
        // Defensive — bizim de tarafımızda extra filter uygula.
        // ============================================================
        var hotels = data.Data.Hotels
            .Where(h => h.Property is not null)
            .Select(MapToSummaryDto)
            .Where(h => MatchesFilters(h, request))
            .ToList();

        _logger.LogInformation(
            "Hotels fetched: API returned {ApiCount}, after client filter {FinalCount}",
            data.Data.Hotels.Count, hotels.Count);

        return new HotelSearchResponseDto
        {
            TotalCount = totalCount,
            DestinationName = destination.Name ?? destination.CityName ?? "Unknown",
            Hotels = hotels,
            HasNextPage = totalCount > (request.PageNumber * hotels.Count)
        };
    }

    /// <summary>
    /// Client-side filter check — API'nin filter eksik uygulaması ihtimaline karşı
    /// her hotel'i tekrar süzeriz. Defensive design.
    /// </summary>
    private static bool MatchesFilters(HotelSummaryDto hotel, HotelSearchRequest request)
    {
        // Price range
        if (request.MinPrice.HasValue && hotel.Price < request.MinPrice.Value)
            return false;

        if (request.MaxPrice.HasValue && hotel.Price > request.MaxPrice.Value)
            return false;

        // Star rating
        if (request.StarRatings is { Count: > 0 } &&
            !request.StarRatings.Contains(hotel.StarRating))
            return false;

        // Review score (API: 70, 80, 90 — DTO: 7.0, 8.0, 9.0)
        if (request.MinReviewScore.HasValue)
        {
            var threshold = request.MinReviewScore.Value / 10.0;
            if (hotel.ReviewScore < threshold)
                return false;
        }

        return true;
    }

    private static int ParseTotalCount(List<RapidMetaDto>? meta)
    {
        if (meta is null || meta.Count == 0) return 0;

        var title = meta[0].Title;
        if (string.IsNullOrWhiteSpace(title)) return 0;

        var firstWord = title.Split(' ').FirstOrDefault();
        return int.TryParse(firstWord, out var count) ? count : 0;
    }

    private static HotelSummaryDto MapToSummaryDto(RapidHotelDto hotel)
    {
        var p = hotel.Property!;
        var price = p.PriceBreakdown?.GrossPrice;
        var strikethrough = p.PriceBreakdown?.StrikethroughPrice;

        var stars = p.AccuratePropertyClass > 0 ? p.AccuratePropertyClass : p.PropertyClass;

        var photoUrl = p.PhotoUrls?.FirstOrDefault();

        return new HotelSummaryDto
        {
            HotelId = hotel.HotelId,
            Name = p.Name ?? "Unknown hotel",
            City = p.WishlistName ?? string.Empty,
            CountryCode = p.CountryCode ?? string.Empty,
            Latitude = p.Latitude,
            Longitude = p.Longitude,
            PhotoUrl = photoUrl,
            StarRating = stars,
            ReviewScore = p.ReviewScore,
            ReviewScoreWord = p.ReviewScoreWord ?? string.Empty,
            ReviewCount = p.ReviewCount,
            Price = (decimal)(price?.Value ?? 0),
            OriginalPrice = strikethrough is not null ? (decimal)strikethrough.Value : null,
            Currency = price?.Currency ?? "USD",
            IsPreferred = p.IsPreferred
        };
    }
    public async Task<HotelDetailDto?> GetHotelDetailAsync(
    long hotelId,
    string? arrivalDate,
    string? departureDate,
    int adults,
    int rooms)
{
    if (hotelId <= 0)
    {
        _logger.LogWarning("Invalid hotel ID: {Id}", hotelId);
        return null;
    }

    try
    {
        // ---- Date validation ----
        if (!DateOnly.TryParse(arrivalDate, CultureInfo.InvariantCulture, out var arrival))
        {
            arrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7));
        }

        if (!DateOnly.TryParse(departureDate, CultureInfo.InvariantCulture, out var departure))
        {
            departure = arrival.AddDays(3);
        }

        if (departure <= arrival)
        {
            departure = arrival.AddDays(1);
        }

        var arrivalStr = arrival.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var departureStr = departure.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        // ---- Query parameters ----
        var queryParams = new Dictionary<string, string>
        {
            ["hotel_id"] = hotelId.ToString(CultureInfo.InvariantCulture),
            ["arrival_date"] = arrivalStr,
            ["departure_date"] = departureStr,
            ["adults"] = adults.ToString(CultureInfo.InvariantCulture),
            ["room_qty"] = rooms.ToString(CultureInfo.InvariantCulture),
            ["units"] = "metric",
            ["temperature_unit"] = "c",
            ["languagecode"] = "en-us",
            ["currency_code"] = "USD"
        };

        var queryString = string.Join("&", queryParams
            .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));

        var url = $"api/v1/hotels/getHotelDetails?{queryString}";

        _logger.LogInformation("Hotel detail URL: {Url}", url);

        var response = await _httpClient.GetAsync(url);

        // ---- Defensive status code handling ----
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            _logger.LogWarning("Booking API rate limit hit for hotel {Id}", hotelId);
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            _logger.LogError("Hotel detail returned {Status}: {Body}",
                response.StatusCode, errorBody);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<RapidHotelDetailResponseDto>(json);

        // ---- API success but data missing (e.g. apartments, villas) ----
        if (data is null || !data.Status || data.Data is null)
        {
            _logger.LogWarning(
                "Hotel detail unavailable for ID {Id} (status={Status}). " +
                "Possibly an apartment or property without detailed listing.",
                hotelId, data?.Status);
            return null;
        }

        return MapToDetailDto(data.Data);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Hotel detail fetch failed for ID {Id}", hotelId);
        return null;
    }
}

private static HotelDetailDto MapToDetailDto(RapidHotelDetailDataDto data)
{
    var stars = data.AccuratePropertyClass > 0
        ? data.AccuratePropertyClass
        : data.PropertyClass;

    var description = data.RawData?.DescriptionTranslations?
        .FirstOrDefault()?.Description ?? string.Empty;

    var facilities = data.FacilitiesBlock?.Facilities?
        .Where(f => !string.IsNullOrWhiteSpace(f.Name))
        .Select(f => f.Name!)
        .Distinct()
        .Take(20)
        .ToList() ?? new List<string>();

    var photos = data.RawData?.PhotoUrls?
        .Where(p => !string.IsNullOrWhiteSpace(p))
        .ToList() ?? new List<string>();

    var pricePerNight = data.CompositePriceBreakdown?.GrossAmountPerNight;
    var totalPrice = data.CompositePriceBreakdown?.GrossAmountTotal;

    return new HotelDetailDto
    {
        HotelId = data.HotelId,
        Name = data.HotelName ?? "Unknown Hotel",
        Address = data.Address ?? string.Empty,
        City = data.CityTranslated ?? data.City ?? string.Empty,
        Country = data.CountryTranslated ?? string.Empty,
        Zip = data.Zip ?? string.Empty,
        Latitude = data.Latitude,
        Longitude = data.Longitude,
        StarRating = stars,
        ReviewScore = data.ReviewScore,
        ReviewScoreWord = data.ReviewScoreWord ?? string.Empty,
        ReviewCount = data.ReviewCount,
        PhotoUrls = photos,
        Description = description,
        Facilities = facilities,
        PricePerNight = pricePerNight is not null ? (decimal?)pricePerNight.Value : null,
        TotalPrice = totalPrice is not null ? (decimal?)totalPrice.Value : null,
        Currency = pricePerNight?.Currency ?? "USD",
        BookingUrl = data.Url ?? string.Empty
    };
}
}