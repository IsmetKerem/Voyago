namespace Voyago.WebUI.Dtos;

public class NewsDto
{
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string? Snippet { get; set; }
    public string? PhotoUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string? SourceLogoUrl { get; set; }
    public DateTime PublishedAtUtc { get; set; }

    public string PublishedAgo
    {
        get
        {
            var diff = DateTime.UtcNow - PublishedAtUtc;
            if (diff.TotalMinutes < 1) return "just now";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
            if (diff.TotalDays < 7) return $"{(int)diff.TotalDays}d ago";
            return PublishedAtUtc.ToString("MMM d");
        }
    }
}