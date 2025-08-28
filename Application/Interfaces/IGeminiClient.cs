using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.DTOs.Gemini.Gemini;

namespace Application.Interfaces
{
    public interface IGeminiClient
    {
        Task<string> GenerateTextAsync(string prompt, CancellationToken ct = default);
    }
}
