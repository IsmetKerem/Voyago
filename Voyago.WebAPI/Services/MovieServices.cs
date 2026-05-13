using System.Text.Json;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class MovieService : IMovieService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MovieService> _logger;


    private static List<RapidMovieDto>? _cachedMovies;
    private static DateTime _cacheTime = DateTime.MinValue;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(6);
    private static readonly object _lock = new();

    public MovieService(HttpClient httpClient, ILogger<MovieService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<MovieDto?> GetRandomTopMovieAsync()
    {
        try
        {
            var movies = await GetMoviesAsync();

            if (movies is null || movies.Count == 0)
            {
                _logger.LogWarning("Movie cache is empty");
                return null;
            }

            var randomIndex = Random.Shared.Next(0, movies.Count);
            var picked = movies[randomIndex];

            return MapToDto(picked);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get random movie");
            return null;
        }
    }

    private async Task<List<RapidMovieDto>?> GetMoviesAsync()
    {
        if (_cachedMovies is not null
            && DateTime.UtcNow - _cacheTime < CacheDuration)
        {
            return _cachedMovies;
        }

        var response = await _httpClient.GetAsync("api/imdb/top250-movies");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var movies = JsonSerializer.Deserialize<List<RapidMovieDto>>(json);

        lock (_lock)
        {
            _cachedMovies = movies;
            _cacheTime = DateTime.UtcNow;
        }

        _logger.LogInformation("Cached {Count} movies", movies?.Count ?? 0);
        return movies;
    }

    private static MovieDto MapToDto(RapidMovieDto rapid)
    {
        return new MovieDto
        {
            Title = rapid.PrimaryTitle ?? "Unknown",
            Description = rapid.Description,
            PosterUrl = rapid.PrimaryImage,
            ImdbUrl = rapid.Url,
            TrailerUrl = rapid.Trailer,
            Year = rapid.StartYear,
            Rating = rapid.AverageRating,
            ContentRating = rapid.ContentRating,
            Genres = rapid.Genres ?? new List<string>(),
            RuntimeMinutes = rapid.RuntimeMinutes
        };
    }
}