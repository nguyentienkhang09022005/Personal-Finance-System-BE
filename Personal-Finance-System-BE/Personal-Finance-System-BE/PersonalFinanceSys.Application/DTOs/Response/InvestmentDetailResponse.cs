namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class InvestmentDetailResponse
    {
        public decimal ValueTotalAsset {  get; set; }

        public decimal AverageNetCost { get; set; }

        public decimal TotalProfitAndLoss { get; set; }

        public List<ListInvestmentDetailResponse> listInvestmentDetailResponses { get; set; }
    }
    public class ListInvestmentDetailResponse
    {
        public Guid IdDetail { get; set; }

        public string? Type { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public decimal Fee { get; set; }

        public decimal Expense { get; set; }

        public decimal CurrentProfit { get; set; }

        public int Profit {  get; set; }

        public DateTime? CreateAt { get; set; }
    }
}
