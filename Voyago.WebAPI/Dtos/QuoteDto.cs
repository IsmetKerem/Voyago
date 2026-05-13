namespace Voyago.WebAPI.Dtos;

public class QuoteDto
{
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;


    public string CategoryFormatted =>
        string.IsNullOrWhiteSpace(Category)
            ? string.Empty
            : char.ToUpper(Category[0]) + Category.Substring(1).ToLower();
}