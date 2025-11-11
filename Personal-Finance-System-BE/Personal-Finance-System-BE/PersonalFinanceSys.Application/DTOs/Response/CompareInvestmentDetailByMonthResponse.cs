namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class CompareInvestmentDetailByMonthResponse
    {
        public MonthlyInvestmentDetailSummary Month1Summary { get; set; } = new();

        public MonthlyInvestmentDetailSummary Month2Summary { get; set; } = new();
    }

    public class MonthlyInvestmentDetailSummary
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public decimal TotalBuy { get; set; }
        public int TotalQuantityBuy { get; set; }
        public List<CompareInvestmentDetailResponse> InvestmentDetailBuyDetails { get; set; } = new();

        public decimal TotalSell { get; set; }
        public int TotalQuantitySell { get; set; }
        public List<CompareInvestmentDetailResponse> InvestmentDetailSellDetails { get; set; } = new();

        public decimal SpreadBuyAndSellByMonth { get; set; }
    }
}
