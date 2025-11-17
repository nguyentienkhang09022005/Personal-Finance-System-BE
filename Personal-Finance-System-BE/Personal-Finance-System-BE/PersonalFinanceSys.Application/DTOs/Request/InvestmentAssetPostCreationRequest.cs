namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentAssetPostCreationRequest
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? UrlImage { get; set; }

        public Guid IdUser { get; set; }

        public List<ListInvestmentAssetOfPost>? InvestmentAssetOfPost { get; set; }
    }

    public class ListInvestmentAssetOfPost
    {
        public string Category { get; set; }

        public string? AssetName { get; set; }

        public string? AssetSymbol { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal MarketCap { get; set; }

        public decimal TotalVolume { get; set; }

        public decimal PriceChangePercentage24h { get; set; }

        public string? Url { get; set; }
    }
}
