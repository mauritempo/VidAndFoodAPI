using Application.DTOs.Gemini;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Net.NetworkInformation;

namespace WineAndFoodAPI.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class GeminiController : ControllerBase
        {
            private readonly IGeminiClient _gemini;

            public GeminiController(IGeminiClient gemini)
            {
                _gemini = gemini;
            }

            // GET simple de demo
            [HttpGet]
            public async Task<IActionResult> GetExplainAi(CancellationToken ct)
            {
                var prompt = "Explain how AI works in a few words";
                var resp = await _gemini.GenerateTextAsync(prompt, ct);

                return Ok(resp);
            }


            [HttpPost]
            public async Task<IActionResult> Generate([FromBody] PromptDto dto, CancellationToken ct)
            {
                if (dto is null || string.IsNullOrWhiteSpace(dto.Prompt))
                    return ValidationProblem("The 'prompt' field is required.");

                var text = await _gemini.GenerateTextAsync(dto.Prompt.Trim(), ct);
            return Ok(new { text });
        }
        }
    }



