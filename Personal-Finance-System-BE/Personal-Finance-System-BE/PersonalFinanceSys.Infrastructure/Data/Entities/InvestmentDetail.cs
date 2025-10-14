using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class InvestmentDetail
{
    public Guid IdDetail { get; set; }

    public string? Type { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public decimal? Expense { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdAsset { get; set; }

    public virtual InvestmentAsset? IdAssetNavigation { get; set; }
}
