using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Budget
{
    public Guid IdBudget { get; set; }

    public string? BudgetName { get; set; }

    public decimal? BudgetGoal { get; set; }

    public Guid IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
