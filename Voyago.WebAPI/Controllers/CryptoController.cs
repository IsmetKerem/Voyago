using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{
   private readonly ICryptoService _cryptoService;

   public CryptoController(ICryptoService cryptoService)
   {
      _cryptoService = cryptoService;
   }

   [HttpGet]
   public async Task<IActionResult> Get()
   {
      var values = await _cryptoService.GetTopCoinsAsync();
      if (values is null)
      {
         return NotFound("Crypto data unavailable");
      }
      return Ok(values);
   }
}