namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class InvestmentFundResponse
    {
        public Guid IdFund { get; set; }

        public string FundName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}
