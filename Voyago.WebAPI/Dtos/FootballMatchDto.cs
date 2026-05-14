namespace Voyago.WebAPI.Dtos;

public class FootballMatchDto
{
    public long Id { get; set; }
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public string StatusLabel { get; set; } = string.Empty;

    public string StatusClass { get; set; } = string.Empty;

    public long TimeTS { get; set; }

    public string ScoreStr => $"{HomeScore} - {AwayScore}";
}