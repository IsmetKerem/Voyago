using Microsoft.AspNetCore.Mvc;
using Voyago.WebAPI.Dtos;
using Voyago.WebAPI.Services;

namespace Voyago.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskAIRequest request)
    {
        // MUTLAKA await!
        var result = await _aiService.AskAsync(request.Question);
        return Ok(result);
    }
}