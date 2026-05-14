using Voyago.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IWeatherService, WeatherService>();

builder.Services.AddHttpClient<ICurrencyService, CurrencyService>(client =>
{
    client.BaseAddress = new Uri("https://currency-conversion-and-exchange-rates.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", builder.Configuration["RapidApi:Key"]!);
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "currency-conversion-and-exchange-rates.p.rapidapi.com");
});
builder.Services.AddHttpClient<ICryptoService, CryptoService>(client =>
{
    client.BaseAddress = new Uri("https://coinranking1.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", builder.Configuration["RapidApi:Key"]!);
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "coinranking1.p.rapidapi.com");
});
builder.Services.AddHttpClient<INewsService, NewsService>(client =>
{
    client.BaseAddress = new Uri("https://real-time-news-data.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key",
        builder.Configuration["RapidApi:Key"]!);
    client.DefaultRequestHeaders.Add("x-rapidapi-host",
        "real-time-news-data.p.rapidapi.com");
});
builder.Services.AddHttpClient<IMovieService, MovieService>(client =>
{
    client.BaseAddress = new Uri("https://imdb236.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key",
        builder.Configuration["RapidApi:Key"]!);
    client.DefaultRequestHeaders.Add("x-rapidapi-host",
        "imdb236.p.rapidapi.com");
});
builder.Services.AddHttpClient<IQuoteService, QuoteService>(client =>
{
    client.BaseAddress = new Uri("https://radio-world-75-000-worldwide-fm-radio-stations.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key",
        builder.Configuration["RapidApi:Key"]!);
    client.DefaultRequestHeaders.Add("x-rapidapi-host",
        "radio-world-75-000-worldwide-fm-radio-stations.p.rapidapi.com");
});
builder.Services.AddHttpClient<IAirQualityService, AirQualityService>(client =>
{
    client.BaseAddress = new Uri("https://air-quality-by-api-ninjas.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key",
        builder.Configuration["RapidApi:Key"]!);
    client.DefaultRequestHeaders.Add("x-rapidapi-host",
        "air-quality-by-api-ninjas.p.rapidapi.com");
});
builder.Services.AddHttpClient<IFootballService, FootballService>(client =>
{
    client.BaseAddress = new Uri("https://free-api-live-football-data.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key",
        builder.Configuration["RapidApi:Key"]!);
    client.DefaultRequestHeaders.Add("x-rapidapi-host",
        "free-api-live-football-data.p.rapidapi.com");
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();