using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Transaction
{
    public Guid IdTransaction { get; set; }

    public string TransactionName { get; set; } = null!;

    public string? TransactionType { get; set; }

    public decimal Amount { get; set; }

    public string? TransactionCategory { get; set; }

    public DateTime? TransactionDate { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
