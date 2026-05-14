namespace Voyago.WebUI.Dtos;

public class HotelSearchRequest
{
    public string Destination { get; set; } = string.Empty;
    public string ArrivalDate { get; set; } = string.Empty;
    public string DepartureDate { get; set; } = string.Empty;
    public int Adults { get; set; } = 2;
    public int Children { get; set; } = 0;
    public int Rooms { get; set; } = 1;
    public int PageNumber { get; set; } = 1;

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<int>? StarRatings { get; set; }
    public int? MinReviewScore { get; set; }
}