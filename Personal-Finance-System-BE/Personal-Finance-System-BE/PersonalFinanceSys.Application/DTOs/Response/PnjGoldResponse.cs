using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class PnjGoldResponse
    {
        [JsonPropertyName("results")]
        public List<PnjGoldItemResponse> Results { get; set; }
    }

    public class PnjGoldItemResponse
    {
        [JsonPropertyName("buy_nhan_24k")]
        public decimal? BuyNhan24K { get; set; }

        [JsonPropertyName("buy_nt_10k")]
        public decimal? BuyNuTrang10K { get; set; }

        [JsonPropertyName("buy_nt_14k")]
        public decimal? BuyNuTrang14K { get; set; }

        [JsonPropertyName("buy_nt_18k")]
        public decimal? BuyNuTrang18K { get; set; }

        [JsonPropertyName("buy_nt_24k")]
        public decimal? BuyNuTrang24K { get; set; }

        [JsonPropertyName("sell_nhan_24k")]
        public decimal? SellNhan24K { get; set; }

        [JsonPropertyName("sell_nt_10k")]
        public decimal? SellNuTrang10K { get; set; }

        [JsonPropertyName("sell_nt_14k")]
        public decimal? SellNuTrang14K { get; set; }

        [JsonPropertyName("sell_nt_18k")]
        public decimal? SellNuTrang18K { get; set; }

        [JsonPropertyName("sell_nt_24k")]
        public decimal? SellNuTrang24K { get; set; }

        [JsonPropertyName("datetime")]
        public long DatetimeUnix { get; set; }
    }
}
