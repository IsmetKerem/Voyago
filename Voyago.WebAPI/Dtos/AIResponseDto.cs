namespace Voyago.WebAPI.Dtos;

public class AIResponseDto
{
    public bool Success { get; set; }
    public string Answer { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}