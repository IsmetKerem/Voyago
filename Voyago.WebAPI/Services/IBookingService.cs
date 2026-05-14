using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IBookingService
{
    Task<HotelSearchResponseDto?> SearchHotelsAsync(HotelSearchRequest request);
}