using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Image
{
    public Guid IdImage { get; set; }

    public string Url { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public Guid IdRef { get; set; }

    public string? RefType { get; set; }
}
