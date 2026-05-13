using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public interface IMovieService
{
   
    Task<MovieDto?> GetRandomTopMovieAsync();
}