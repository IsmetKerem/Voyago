namespace Voyago.WebUI.Dtos;

public class AIResponseDto
{
    public bool Success { get; set; }
    public string Answer { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}