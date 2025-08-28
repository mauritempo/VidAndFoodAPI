using Application.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Application.DTOs.Gemini.Gemini;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Services
{
    public class GeminiApiService: IGeminiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GeminiOptions _opts;

        public GeminiApiService(IHttpClientFactory httpClientFactory, IOptions<GeminiOptions> opts)
        {
            _httpClientFactory = httpClientFactory;
            _opts = opts.Value;
        }


        public async Task<string> GenerateTextAsync(string prompt, CancellationToken ct = default)
        {
            var client = _httpClientFactory.CreateClient("geminiHttpClient");
            var path = $"models/{_opts.Model}:generateContent";

            var request = new GeminiGenerateContentRequest
            {
                Contents = new()
                {
                    new GeminiContent
                    {
                        Parts = new() { new GeminiTextPart { Text = prompt } }
                    }
                }
            };

            using var httpReq = new HttpRequestMessage(HttpMethod.Post, path);
            httpReq.Headers.Add("x-goog-api-key", _opts.ApiKey);
            httpReq.Content = JsonContent.Create(request);

            using var httpResp = await client.SendAsync(httpReq, ct);
            httpResp.EnsureSuccessStatusCode();

            var result = await httpResp.Content.ReadFromJsonAsync<GeminiGenerateContentResponse>(cancellationToken: ct);

            // EXTRAER TEXTO: candidates[0].content.parts[].text
            var text =
                result?.Candidates?
                      .FirstOrDefault()?
                      .Content?
                      .Parts?
                      .Select(p => p?.Text)
                      .Where(s => !string.IsNullOrWhiteSpace(s))
                      .ToArray();

            return (text is { Length: > 0 })
                ? string.Concat(text)       // concatena todas las parts de texto
                : string.Empty;             // fallback seguro
        }
    }
}
