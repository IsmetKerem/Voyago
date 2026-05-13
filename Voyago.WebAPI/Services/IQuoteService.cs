using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IQuoteService
{
    Task<List<QuoteDto>> GetRandomQuotesAsync(int count = 2);
}