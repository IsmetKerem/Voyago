using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface ICurrencyService
{
    Task<CurrencyDto?> GetLatestRatesAsync();
}