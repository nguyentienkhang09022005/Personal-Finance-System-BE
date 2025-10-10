using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class InvestmentDetail
{
    public Guid IdInvestment { get; set; }

    public string InvestmentName { get; set; } = null!;

    public string? InvestmentType { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdFund { get; set; }

    public Guid? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual InvestmentFund? IdFundNavigation { get; set; }
}
