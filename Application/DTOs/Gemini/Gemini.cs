using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Gemini
{
    public class Gemini
    {
        public class GeminiTextPart
        {
            public string Text { get; set; } = "";
        }

        public class GeminiContent
        {
            public List<GeminiTextPart> Parts { get; set; } = new();
        }

        public class GeminiGenerateContentRequest
        {
            public List<GeminiContent> Contents { get; set; } = new();
        }

        // Response (simplificado: ajusta si necesitás más campos)
        public class GeminiCandidate
        {
            public GeminiContent Content { get; set; } = new();
            public string? FinishReason { get; set; }
        }

        public class GeminiGenerateContentResponse
        {
            public List<GeminiCandidate> Candidates { get; set; } = new();
        }
    }
}
