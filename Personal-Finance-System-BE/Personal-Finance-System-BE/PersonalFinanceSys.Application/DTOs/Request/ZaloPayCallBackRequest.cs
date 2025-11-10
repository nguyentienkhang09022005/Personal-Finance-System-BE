using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class ZaloPayCallBackRequest
    {
        [JsonPropertyName("data")]
        public string? Data { get; set; }

        [JsonPropertyName("mac")]
        public string? Mac { get; set; }
    }
}
