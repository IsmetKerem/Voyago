using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface ICryptoService
{
    Task<CryptoDto?> GetTopCoinsAsync();
}