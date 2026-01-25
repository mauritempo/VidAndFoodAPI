using Domain.Interfaces;
using GenerativeAI;
using GenerativeAI.Types;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServicesEXT
{
    public class GeminiAPIService: IGeminiApiService
    {
        private readonly GenerativeModel _model;
        // El framework de .NET inyectará la instancia configurada
        public GeminiAPIService(IHttpClientFactory httpClientFactory, IOptions<GeminiOptions> options)
        {
            var cfg = options.Value;
            var client = httpClientFactory.CreateClient("Gemini");


            _model = new GenerativeModel(cfg.Model, cfg.ApiKey);
        }


        public async Task<string> GenerateContentAsync(string prompt, CancellationToken ct = default)
        {
            try
            {
                var response = await _model.GenerateContentAsync(prompt, ct);
                return response?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Logueá con tu logger (no Console) en producción
                Console.WriteLine($"Gemini error: {ex.Message}");
                return "Lo siento, no pude obtener una respuesta en este momento.";
            }
        }


    }
}
