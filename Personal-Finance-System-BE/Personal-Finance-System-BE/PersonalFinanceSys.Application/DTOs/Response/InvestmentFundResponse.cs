namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class InvestmentFundResponse
    {
        public Guid IdFund { get; set; }

        public string FundName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public List<ListInvestmentAssetResponse> listInvestmentAssetResponses { get; set; }
    }

    public class CalculateTotalFinanceAndProfitResponse
    {
        public decimal TotalFinanceCurrent {  get; set; }

        public decimal TotalProfitAndLoss { get; set; }
    }

    public class CalculateTotalTransactionResponse
    {
        public decimal TotalTransactionAmount { get; set; }
    }

    public class AverageFinanceAssetResponse
    {
        public string AssetName { get; set; }
        public decimal AverageFinance { get; set; }  
        public decimal PercentageInPortfolio { get; set; }
    }

}
