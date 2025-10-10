using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Favorite
{
    public Guid IdFavorite { get; set; }

    public bool? IsFavorite { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdPost { get; set; }

    public Guid? IdUser { get; set; }

    public virtual Post? IdPostNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
