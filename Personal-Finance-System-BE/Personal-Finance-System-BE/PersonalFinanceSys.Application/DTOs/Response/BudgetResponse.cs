namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class BudgetResponse
    {
        public Guid IdBudget { get; set; }

        public string? BudgetName { get; set; }

        public decimal CurrentBudget { get; set; }

        public double BudgetProgress { get; set; }

        public decimal BudgetGoal { get; set; }

        public string? UrlImage { get; set; }
    }
}
