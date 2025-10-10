using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Category
{
    public Guid IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<InvestmentDetail> InvestmentDetails { get; set; } = new List<InvestmentDetail>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
