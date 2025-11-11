namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class CompareInvestmentDetailByYearResponse
    {
        public YearlyInvestmentDetailSummary Year1Summary { get; set; } = new();

        public YearlyInvestmentDetailSummary Year2Summary { get; set; } = new();
    }

    public class YearlyInvestmentDetailSummary
    {
        public int Year { get; set; }

        public decimal TotalBuy { get; set; }
        public int TotalQuantityBuy { get; set; }
        public List<CompareInvestmentDetailResponse> InvestmentDetailBuyDetails { get; set; } = new();

        public decimal TotalSell { get; set; }
        public int TotalQuantitySell { get; set; }
        public List<CompareInvestmentDetailResponse> InvestmentDetailSellDetails { get; set; } = new();

        public decimal SpreadBuyAndSellByYear { get; set; }
    }

    public class CompareInvestmentDetailResponse
    {
        public Guid IdDetail { get; set; }

        public string? Type { get; set; }

        public decimal? Price { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Fee { get; set; }

        public decimal? Expense { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}
