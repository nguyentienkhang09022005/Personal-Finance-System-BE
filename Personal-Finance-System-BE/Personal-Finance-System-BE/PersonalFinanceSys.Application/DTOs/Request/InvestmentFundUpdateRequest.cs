namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentFundUpdateRequest
    {
        public string? FundName { get; set; }

        public string? Description { get; set; }

        public string? UrlImage { get; set; }
    }
}
