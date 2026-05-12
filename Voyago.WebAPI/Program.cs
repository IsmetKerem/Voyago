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