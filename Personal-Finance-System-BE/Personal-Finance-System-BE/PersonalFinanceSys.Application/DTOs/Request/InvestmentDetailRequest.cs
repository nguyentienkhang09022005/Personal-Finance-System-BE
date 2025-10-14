namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentDetailRequest
    {
        public string? Type { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public decimal? Expense { get; set; }

        public Guid IdAsset { get; set; }
    }
}
