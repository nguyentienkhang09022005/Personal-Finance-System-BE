namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class InvestmentAssetResponse
    {
        public decimal TotalFinanceCurrent { get; set; }

        public decimal TotalTransactionAmount { get; set; }

        public decimal TotalProfitAndLoss { get; set; }

        public List<AverageFinanceAssetResponse> AverageFinanceAssets { get; set; }

        public List<ListInvestmentAssetResponse> listInvestmentAssetResponse { get; set; }
    }

    public class ListInvestmentAssetResponse
    {
        public Guid IdAsset { get; set; }

        public string? Id { get; set; }

        public string? AssetName { get; set; }

        public string? AssetSymbol { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal MarketCap { get; set; }

        public decimal TotalVolume { get; set; }

        public decimal PriceChangePercentage24h { get; set; }

        public string? Url { get; set; }
    }
}
