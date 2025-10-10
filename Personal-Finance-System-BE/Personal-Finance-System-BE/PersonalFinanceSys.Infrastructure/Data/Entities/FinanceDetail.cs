using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class FinanceDetail
{
    public Guid IdFinance { get; set; }

    public Guid? IdUser { get; set; }

    public string FinanceName { get; set; } = null!;

    public decimal? Amount { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
