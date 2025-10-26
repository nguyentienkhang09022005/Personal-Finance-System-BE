using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class SavingDetail
{
    public Guid IdDetail { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? IdSaving { get; set; }

    public virtual SavingGoal? IdSavingNavigation { get; set; }
}
