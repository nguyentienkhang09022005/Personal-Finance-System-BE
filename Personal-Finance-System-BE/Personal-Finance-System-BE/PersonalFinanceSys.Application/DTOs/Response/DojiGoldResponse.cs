using System.Text.Json.Serialization;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class DojiGoldResponse
    {
        [JsonPropertyName("results")]
        public List<DojiGoldItemResponse> Results { get; set; }
    }

    public class DojiGoldItemResponse
    {
        [JsonPropertyName("buy_hn")]
        public decimal? BuyHaNoi { get; set; }

        [JsonPropertyName("buy_hcm")]
        public decimal? BuyHoChiMinh { get; set; }

        [JsonPropertyName("buy_dn")]
        public decimal? BuyDaNang { get; set; }

        [JsonPropertyName("buy_ct")]
        public decimal? BuyCanTho { get; set; }

        [JsonPropertyName("sell_hn")]
        public decimal? SellHaNoi { get; set; }

        [JsonPropertyName("sell_hcm")]
        public decimal? SellHoChiMinh { get; set; }

        [JsonPropertyName("sell_dn")]
        public decimal? SellDaNang { get; set; }

        [JsonPropertyName("sell_ct")]
        public decimal? SellCanTho { get; set; }

        [JsonPropertyName("datetime")]
        public long DatetimeUnix { get; set; }
    }
}
