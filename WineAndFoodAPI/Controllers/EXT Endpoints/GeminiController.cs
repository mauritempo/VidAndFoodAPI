using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WineAndFoodAPI.Controllers.EXT_Endpoints
{
    [ApiController]
    [Route("api/gemini")]
    public class GeminiController : Controller
    {

        private readonly IGeminiApiService _gemini;

        public GeminiController(IGeminiApiService gemini)
        {
            _gemini = gemini;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] PromptDto dto, CancellationToken ct)
        {
            var text = await _gemini.GenerateContentAsync(dto.Prompt, ct);
            return Ok(new { text });
        }
    }

    public record PromptDto(string Prompt);
}

