using Voyago.WebUI.Dtos;
using Voyago.WebUI.Dtos;

namespace Voyago.WebUI.Services;

public class VoyagoApiClient : IVoyagoApiClient
{
    private readonly HttpClient _httpClient;

    public VoyagoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherDto?> GetWeatherAsync(string city)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<WeatherDto>(
                $"api/weather?city={Uri.EscapeDataString(city)}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
    public async Task<CurrencyDto?> GetCurrencyAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CurrencyDto>("api/currency");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Currency error: {ex.Message}");
            return null;
        }
    }

    public async Task<CryptoDto?> GetCryptoAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CryptoDto>("api/Crypto");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Crypto error: {ex.Message}");
            return null;
           
        }
    }
    public async Task<List<NewsDto>> GetTopHeadlinesAsync(int limit = 5)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/news/top-headlines?limit={limit}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[VoyagoApiClient] News API returned status {response.StatusCode}");
                return new List<NewsDto>();
            }

            var news = await response.Content.ReadFromJsonAsync<List<NewsDto>>();
            return news ?? new List<NewsDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] News error: {ex.Message}");
            return new List<NewsDto>();
        }
    }
    public async Task<MovieDto?> GetRandomMovieAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MovieDto>("api/movie/random");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Movie error: {ex.Message}");
            return null;
        }
    }
    public async Task<List<QuoteDto>> GetRandomQuotesAsync(int count = 2)
    {
        try
        {
            var result = await _httpClient
                .GetFromJsonAsync<List<QuoteDto>>($"api/quote/random?count={count}");

            return result ?? new List<QuoteDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Quote error: {ex.Message}");
            return new List<QuoteDto>();
        }
    }
    public async Task<AirQualityDto?> GetAirQualityAsync(string city)
    {
        try
        {
            var encodedCity = Uri.EscapeDataString(city);
            return await _httpClient
                .GetFromJsonAsync<AirQualityDto>($"api/airquality?city={encodedCity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] AirQuality error: {ex.Message}");
            return null;
        }
    }
    public async Task<List<FootballMatchDto>> GetTopMatchesAsync(int count = 4)
    {
        try
        {
            var result = await _httpClient
                .GetFromJsonAsync<List<FootballMatchDto>>($"api/football/top-matches?count={count}");

            return result ?? new List<FootballMatchDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] Football error: {ex.Message}");
            return new List<FootballMatchDto>();
        }
    }
    public async Task<HotelSearchResponseDto?> SearchHotelsAsync(HotelSearchRequest request)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"destination={Uri.EscapeDataString(request.Destination)}",
                $"arrivalDate={Uri.EscapeDataString(request.ArrivalDate)}",
                $"departureDate={Uri.EscapeDataString(request.DepartureDate)}",
                $"adults={request.Adults}",
                $"children={request.Children}",
                $"rooms={request.Rooms}",
                $"pageNumber={request.PageNumber}"
            };

            if (request.MinPrice.HasValue)
                queryParams.Add($"minPrice={request.MinPrice.Value}");

            if (request.MaxPrice.HasValue)
                queryParams.Add($"maxPrice={request.MaxPrice.Value}");

            if (request.StarRatings is { Count: > 0 })
            {
                foreach (var star in request.StarRatings)
                    queryParams.Add($"starRatings={star}");
            }

            if (request.MinReviewScore.HasValue)
                queryParams.Add($"minReviewScore={request.MinReviewScore.Value}");

            var queryString = string.Join("&", queryParams);
            var url = $"api/booking/search?{queryString}";

            return await _httpClient.GetFromJsonAsync<HotelSearchResponseDto>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] SearchHotels error: {ex.Message}");
            return null;
        }
    }
    public async Task<AIResponseDto?> AskAiAsync(string question)
    {
        try
        {
            var request = new AskAIRequest { Question = question };
            var response = await _httpClient.PostAsJsonAsync("api/ai/ask", request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[VoyagoApiClient] AI ask returned {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<AIResponseDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] AI ask error: {ex.Message}");
            return null;
        }
    }
    public async Task<HotelDetailDto?> GetHotelDetailAsync(
        long hotelId,
        string? arrivalDate,
        string? departureDate,
        int adults = 2,
        int rooms = 1)
    {
        try
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(arrivalDate))
                query.Add($"arrivalDate={Uri.EscapeDataString(arrivalDate)}");

            if (!string.IsNullOrWhiteSpace(departureDate))
                query.Add($"departureDate={Uri.EscapeDataString(departureDate)}");

            query.Add($"adults={adults}");
            query.Add($"rooms={rooms}");

            var url = $"api/booking/hotel/{hotelId}?{string.Join("&", query)}";

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"[VoyagoApiClient] Hotel {hotelId} not found");
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[VoyagoApiClient] GetHotelDetail returned {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<HotelDetailDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VoyagoApiClient] GetHotelDetail error: {ex.Message}");
            return null;
        }
    }
    
    
}