using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]

        public List<GeminiCandidate> Candidates { get; set; }
    }

    public class GeminiCandidate
    {
        [JsonPropertyName("content")]

        public GeminiContent Content { get; set; }
    }
}
