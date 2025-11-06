using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Permission
{
    public string PermissionName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Role> RoleNames { get; set; } = new List<Role>();
}
