namespace Voyago.WebUI.Dtos;

public class HotelSearchResponseDto
{
    public int TotalCount { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public List<HotelSummaryDto> Hotels { get; set; } = new();
    public bool HasNextPage { get; set; }
}