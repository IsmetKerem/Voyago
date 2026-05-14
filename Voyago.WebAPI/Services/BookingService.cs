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

        if (request.MinPrice.HasValue && request.MaxPrice.HasValue)
        {
            queryParams["price_min"] = request.MinPrice.Value.ToString("0", CultureInfo.InvariantCulture);
            queryParams["price_max"] = request.MaxPrice.Value.ToString("0", CultureInfo.InvariantCulture);
        }

        if (request.StarRatings is { Count: > 0 })
        {
            queryParams["class"] = string.Join(",", request.StarRatings);
        }

        if (request.MinReviewScore.HasValue)
        {
            queryParams["review_score"] = request.MinReviewScore.Value.ToString();
        }

        var queryString = string.Join("&", queryParams
            .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));

        var url = $"api/v1/hotels/searchHotels?{queryString}";

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

        var hotels = data.Data.Hotels
            .Where(h => h.Property is not null)
            .Select(MapToSummaryDto)
            .ToList();

        return new HotelSearchResponseDto
        {
            TotalCount = totalCount,
            DestinationName = destination.Name ?? destination.CityName ?? "Unknown",
            Hotels = hotels,
            HasNextPage = totalCount > (request.PageNumber * hotels.Count)
        };
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
}