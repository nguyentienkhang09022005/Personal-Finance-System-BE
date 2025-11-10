using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Package
{
    public Guid IdPackage { get; set; }

    public string PackageName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Permission> PermissionNames { get; set; } = new List<Permission>();
}
