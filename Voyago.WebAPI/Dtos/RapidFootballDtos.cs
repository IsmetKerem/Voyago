using System.Text.Json.Serialization;

namespace Voyago.WebAPI.Dtos;


public class RapidLiveResponseDto
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("response")]
    public RapidLiveDataDto? Response { get; set; }
}

public class RapidLiveDataDto
{
    [JsonPropertyName("live")]
    public List<RapidMatchDto>? Live { get; set; }
}

// /football-get-matches-by-date response
public class RapidMatchesByDateResponseDto
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("response")]
    public RapidMatchesByDateDataDto? Response { get; set; }
}

public class RapidMatchesByDateDataDto
{
    [JsonPropertyName("matches")]
    public List<RapidMatchDto>? Matches { get; set; }
}


public class RapidMatchDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("leagueId")]
    public long LeagueId { get; set; }

    // String tarih, "14.05.2026 19:00" formatında
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("home")]
    public RapidTeamDto? Home { get; set; }

    [JsonPropertyName("away")]
    public RapidTeamDto? Away { get; set; }

    [JsonPropertyName("status")]
    public RapidStatusDto? Status { get; set; }

    [JsonPropertyName("timeTS")]
    public long TimeTS { get; set; }  // unix milliseconds
}

public class RapidTeamDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("longName")]
    public string? LongName { get; set; }
}

public class RapidStatusDto
{
    [JsonPropertyName("scoreStr")]
    public string? ScoreStr { get; set; }

    [JsonPropertyName("finished")]
    public bool Finished { get; set; }

    [JsonPropertyName("started")]
    public bool Started { get; set; }

    [JsonPropertyName("ongoing")]
    public bool Ongoing { get; set; }

    [JsonPropertyName("cancelled")]
    public bool Cancelled { get; set; }

    [JsonPropertyName("liveTime")]
    public RapidLiveTimeDto? LiveTime { get; set; }

    [JsonPropertyName("reason")]
    public RapidStatusReasonDto? Reason { get; set; }
}

public class RapidLiveTimeDto
{
 
    [JsonPropertyName("short")]
    public string? Short { get; set; }
}

public class RapidStatusReasonDto
{
    [JsonPropertyName("short")]
    public string? Short { get; set; }
}