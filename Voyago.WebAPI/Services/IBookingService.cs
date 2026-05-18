using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IBookingService
{
    Task<HotelSearchResponseDto?> SearchHotelsAsync(HotelSearchRequest request);

    // YENİ
    Task<HotelDetailDto?> GetHotelDetailAsync(
        long hotelId,
        string? arrivalDate,
        string? departureDate,
        int adults,
        int rooms);
}