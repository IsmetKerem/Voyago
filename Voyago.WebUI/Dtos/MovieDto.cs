namespace Voyago.WebUI.Dtos;

public class MovieDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PosterUrl { get; set; }
    public string? ImdbUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public int? Year { get; set; }
    public double? Rating { get; set; }
    public string? ContentRating { get; set; }
    public List<string> Genres { get; set; } = new();
    public int? RuntimeMinutes { get; set; }

    public string RuntimeFormatted
    {
        get
        {
            if (RuntimeMinutes is null || RuntimeMinutes <= 0) return "—";
            var hours = RuntimeMinutes.Value / 60;
            var minutes = RuntimeMinutes.Value % 60;
            return hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
        }
    }

    public string GenresFormatted =>
        Genres.Any() ? string.Join(" · ", Genres.Take(3)) : string.Empty;
}