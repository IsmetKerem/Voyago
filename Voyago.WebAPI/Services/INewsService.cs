using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface INewsService
{
    Task<List<NewsDto>> GetTopHeadlinesAsync(int limit = 5);
}