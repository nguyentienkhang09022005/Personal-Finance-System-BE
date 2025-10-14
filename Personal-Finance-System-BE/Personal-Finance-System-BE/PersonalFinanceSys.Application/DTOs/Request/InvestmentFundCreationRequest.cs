namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentFundCreationRequest
    {

        public string FundName { get; set; } = null!;

        public string? Description { get; set; }

        public Guid IdUser { get; set; }
    }
}
