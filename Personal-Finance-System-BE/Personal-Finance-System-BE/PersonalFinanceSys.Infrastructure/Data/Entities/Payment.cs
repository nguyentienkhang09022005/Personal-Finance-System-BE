using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Payment
{
    public Guid IdPayment { get; set; }

    public decimal? Amount { get; set; }

    public string? Method { get; set; }

    public string? Status { get; set; }

    public string? IdAppTrans { get; set; }

    public int? IdZpTrans { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdUser { get; set; }

    public Guid? IdPackage { get; set; }

    public virtual Package? IdPackageNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
