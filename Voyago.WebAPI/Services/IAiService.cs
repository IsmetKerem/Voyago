using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IAiService
{
    Task<AIResponseDto> AskAsync(string question);
}