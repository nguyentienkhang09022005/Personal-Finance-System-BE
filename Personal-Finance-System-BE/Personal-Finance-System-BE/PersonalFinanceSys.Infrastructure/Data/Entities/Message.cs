using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Message
{
    public Guid IdMessage { get; set; }

    public string Content { get; set; } = null!;

    public bool? IsFriend { get; set; }

    public DateTime? SendAt { get; set; }

    public Guid? IdFriendship { get; set; }

    public Guid? IdUser { get; set; }

    public virtual Friendship? IdFriendshipNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
