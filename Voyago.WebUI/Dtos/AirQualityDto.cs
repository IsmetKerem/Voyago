namespace Voyago.WebUI.Dtos;

public class AirQualityDto
{
    public string City { get; set; } = string.Empty;
    public int OverallAqi { get; set; }

    public double Pm25Concentration { get; set; }
    public double Pm10Concentration { get; set; }
    public double O3Concentration { get; set; }
    public double No2Concentration { get; set; }
    public double SO2Concentration { get; set; }
    public double COConcentration { get; set; }

    public string AqiCategory => OverallAqi switch
    {
        <= 50 => "Good",
        <= 100 => "Moderate",
        <= 150 => "Unhealthy for sensitive",
        <= 200 => "Unhealthy",
        <= 300 => "Very unhealthy",
        _ => "Hazardous"
    };

    public string AqiColorClass => OverallAqi switch
    {
        <= 50 => "aqi-good",
        <= 100 => "aqi-moderate",
        <= 150 => "aqi-sensitive",
        <= 200 => "aqi-unhealthy",
        <= 300 => "aqi-very-unhealthy",
        _ => "aqi-hazardous"
    };

    public string AqiAdvice => OverallAqi switch
    {
        <= 50 => "Air is clean. Enjoy outdoor activities.",
        <= 100 => "Air is acceptable. Sensitive groups take care.",
        <= 150 => "Sensitive groups should reduce outdoor activity.",
        <= 200 => "Everyone may experience effects.",
        <= 300 => "Avoid prolonged outdoor exertion.",
        _ => "Health emergency. Stay indoors."
    };
}