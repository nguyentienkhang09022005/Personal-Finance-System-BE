using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class InvestmentFund
{
    public Guid IdFund { get; set; }

    public string FundName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<InvestmentAsset> InvestmentAssets { get; set; } = new List<InvestmentAsset>();
}
