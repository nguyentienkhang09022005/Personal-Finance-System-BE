using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class InfCryptoResponse
    {
        [JsonPropertyName("market_data")]
        public MarketData MarketData { get; set; }
    }
    public class MarketData
    {
        [JsonPropertyName("current_price")]
        public CurrentPrice CurrentPrice { get; set; }
    }

    public class CurrentPrice
    {
        [JsonPropertyName("vnd")]
        public decimal VND { get; set; }
    }
}
