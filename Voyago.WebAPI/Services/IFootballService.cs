using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IFootballService
{
    Task<List<FootballMatchDto>> GetTopMatchesAsync(int count = 4);
}