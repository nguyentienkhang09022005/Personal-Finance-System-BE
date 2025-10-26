namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class BudgetCreationRequest
    {
        public string? BudgetName { get; set; }

        public decimal BudgetGoal { get; set; }

        public string? UrlImage { get; set; }

        public Guid? IdUser { get; set; }
    }
}
