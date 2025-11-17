using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Post
{
    public Guid IdPost { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public Guid? IdUser { get; set; }

    public bool? IsApproved { get; set; }

    public string? Snapshot { get; set; }

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual User? IdUserNavigation { get; set; }
}
