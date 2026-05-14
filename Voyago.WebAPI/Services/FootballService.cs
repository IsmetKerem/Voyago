using System.Globalization;
using System.Text.Json;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class FootballService : IFootballService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FootballService> _logger;


    private static readonly HashSet<string> TopClubs = new(StringComparer.OrdinalIgnoreCase)
    {
        // La Liga
        "Real Madrid", "Barcelona", "Atletico Madrid", "Valencia", "Sevilla",

        // Premier League
        "Manchester United", "Manchester City", "Liverpool",
        "Arsenal", "Chelsea", "Tottenham Hotspur",

        // Bundesliga
        "Bayern München", "Borussia Dortmund", "Bayer Leverkusen",

        // Ligue 1
        "Paris Saint-Germain", "Marseille", "Monaco",

        // Serie A
        "Juventus", "Inter", "AC Milan", "Roma", "Napoli", "Atalanta"
    };
    
    private static List<FootballMatchDto>? _cachedMatches;
    private static DateTime _cacheTime = DateTime.MinValue;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);
    private static readonly object _lock = new();

    public FootballService(HttpClient httpClient, ILogger<FootballService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<FootballMatchDto>> GetTopMatchesAsync(int count = 4)
    {
        if (_cachedMatches is not null
            && DateTime.UtcNow - _cacheTime < CacheDuration)
        {
            return _cachedMatches.Take(count).ToList();
        }

        try
        {
            var liveMatches = await FetchLiveMatchesAsync();

            var filteredLive = liveMatches
                .Where(IsTopClubMatch)
                .ToList();

            _logger.LogInformation("Live matches found: {Total}, filtered: {Filtered}",
                liveMatches.Count, filteredLive.Count);

            List<RapidMatchDto> finalMatches;

            if (filteredLive.Count >= count)
            {
                finalMatches = filteredLive;
            }
            else
            {
                var todayMatches = await FetchTodayMatchesAsync();
                var filteredToday = todayMatches
                    .Where(IsTopClubMatch)
                    .ToList();

                _logger.LogInformation("Today matches found: {Total}, filtered: {Filtered}",
                    todayMatches.Count, filteredToday.Count);

                finalMatches = filteredLive
                    .Concat(filteredToday)
                    .GroupBy(m => m.Id)
                    .Select(g => g.First())
                    .ToList();
            }

            var sorted = finalMatches
                .OrderByDescending(m => m.Status?.Ongoing ?? false)
                .ThenBy(m => m.TimeTS)
                .Take(count)
                .Select(MapToDto)
                .ToList();

            lock (_lock)
            {
                _cachedMatches = sorted;
                _cacheTime = DateTime.UtcNow;
            }

            return sorted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Football service failed");
            return new List<FootballMatchDto>();
        }
    }

    private async Task<List<RapidMatchDto>> FetchLiveMatchesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("football-current-live");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<RapidLiveResponseDto>(json);

            return data?.Response?.Live ?? new List<RapidMatchDto>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Live matches fetch failed");
            return new List<RapidMatchDto>();
        }
    }

    private async Task<List<RapidMatchDto>> FetchTodayMatchesAsync()
    {
        try
        {
            var today = DateTime.UtcNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            var response = await _httpClient.GetAsync($"football-get-matches-by-date?date={today}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<RapidMatchesByDateResponseDto>(json);

            return data?.Response?.Matches ?? new List<RapidMatchDto>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Today matches fetch failed");
            return new List<RapidMatchDto>();
        }
    }

    private static bool IsTopClubMatch(RapidMatchDto match)
    {
        if (match.Home?.LongName is null || match.Away?.LongName is null)
            return false;

        return TopClubs.Contains(match.Home.LongName)
            || TopClubs.Contains(match.Away.LongName);
    }

    private static FootballMatchDto MapToDto(RapidMatchDto rapid)
    {
        var (label, cssClass) = ComputeStatus(rapid.Status);

        return new FootballMatchDto
        {
            Id = rapid.Id,
            HomeTeam = rapid.Home?.LongName ?? "?",
            AwayTeam = rapid.Away?.LongName ?? "?",
            HomeScore = rapid.Home?.Score ?? 0,
            AwayScore = rapid.Away?.Score ?? 0,
            StatusLabel = label,
            StatusClass = cssClass,
            TimeTS = rapid.TimeTS
        };
    }

    private static (string Label, string CssClass) ComputeStatus(RapidStatusDto? status)
    {
        if (status is null)
            return ("—", "match-unknown");

        if (status.Cancelled)
            return ("Postponed", "match-postponed");

        if (status.Ongoing == true && status.LiveTime?.Short is not null)
        {
            return (status.LiveTime.Short, "match-live");
        }

        if (status.Finished)
        {
            var label = status.Reason?.Short ?? "FT";
            return (label, "match-finished");
        }

        if (!status.Started)
            return ("Soon", "match-upcoming");

        return ("—", "match-unknown");
    }
}