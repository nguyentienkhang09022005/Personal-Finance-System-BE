using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Role
{
    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Permission> PermissionNames { get; set; } = new List<Permission>();
}
