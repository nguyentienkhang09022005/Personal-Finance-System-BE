namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class BudgetDomain
    {
        public Guid IdBudget { get; set; }

        public string? BudgetName { get; set; }

        public decimal BudgetGoal { get; set; }

        public Guid? IdUser { get; set; }

        public BudgetDomain() { }

        public BudgetDomain(decimal bugetGoal) 
        {
            SetBugetGoal(bugetGoal);
        }

        public void SetBugetGoal(decimal bugetGoal)
        {
            if (bugetGoal < 0)
            {
                throw new ArgumentException("Số tiền ngân sách không được nhỏ hơn 0!");
            }
            BudgetGoal = bugetGoal;
        }
    }
}
