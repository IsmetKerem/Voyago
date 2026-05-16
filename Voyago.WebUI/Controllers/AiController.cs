using Microsoft.AspNetCore.Mvc;
using Voyago.WebUI.Dtos;
using Voyago.WebUI.Services;

namespace Voyago.WebUI.Controllers;

[Route("[controller]")]
public class AiController : Controller
{
    private readonly IVoyagoApiClient _apiClient;

    public AiController(IVoyagoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    
    [HttpPost("Ask")]
    public async Task<IActionResult> Ask([FromBody] AskAIRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Question))
        {
            return BadRequest(new AIResponseDto
            {
                Success = false,
                ErrorMessage = "Please ask a question."
            });
        }

        var result = await _apiClient.AskAiAsync(request.Question);

        if (result is null)
        {
            return Ok(new AIResponseDto
            {
                Success = false,
                ErrorMessage = "AI service is unavailable."
            });
        }

        return Ok(result);
    }
}