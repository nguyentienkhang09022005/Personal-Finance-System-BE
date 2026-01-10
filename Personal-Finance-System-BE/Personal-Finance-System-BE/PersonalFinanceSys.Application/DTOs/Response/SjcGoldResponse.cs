using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class SjcGoldResponse
    {
        [JsonPropertyName("results")]
        public List<SjcGoldItemResponse> Results { get; set; }
    }

    public class SjcGoldItemResponse
    {
        [JsonPropertyName("buy_1c")]
        public decimal? Buy1Chi { get; set; }

        [JsonPropertyName("buy_5c")]
        public decimal? Buy5Chi { get; set; }

        [JsonPropertyName("buy_1l")]
        public decimal? Buy1Luong { get; set; }

        [JsonPropertyName("buy_nhan1c")]
        public decimal? BuyNhan1Chi { get; set; }

        [JsonPropertyName("buy_nutrang_75")]
        public decimal? BuyNuTrang75 { get; set; }

        [JsonPropertyName("buy_nutrang_99")]
        public decimal? BuyNuTrang99 { get; set; }

        [JsonPropertyName("buy_nutrang_9999")]
        public decimal? BuyNuTrang9999 { get; set; }

        [JsonPropertyName("sell_1c")]
        public decimal? Sell1Chi { get; set; }

        [JsonPropertyName("sell_5c")]
        public decimal? Sell5Chi { get; set; }

        [JsonPropertyName("sell_1l")]
        public decimal? Sell1Luong { get; set; }

        [JsonPropertyName("sell_nhan1c")]
        public decimal? SellNhan1Chi { get; set; }

        [JsonPropertyName("sell_nutrang_75")]
        public decimal? SellNuTrang75 { get; set; }

        [JsonPropertyName("sell_nutrang_99")]
        public decimal? SellNuTrang99 { get; set; }

        [JsonPropertyName("sell_nutrang_9999")]
        public decimal? SellNuTrang9999 { get; set; }

        [JsonPropertyName("datetime")]
        public long DatetimeUnix { get; set; }
    }
}
