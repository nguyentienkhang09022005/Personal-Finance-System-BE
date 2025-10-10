using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class SavingGoal
{
    public Guid IdSaving { get; set; }

    public string SavingName { get; set; } = null!;

    public decimal? CurrentAmount { get; set; }

    public decimal? TargetAmount { get; set; }

    public DateOnly? TargetDate { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
