namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class InvestmentFundDomain
    {
        public Guid IdFund { get; set; }

        public string FundName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdUser { get; set; }
    }
}
