namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentFundCreationRequest
    {

        public string? FundName { get; set; }

        public string? Description { get; set; }

        public string? UrlImage { get; set; }

        public Guid IdUser { get; set; }
    }
}
