using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class User
{
    public Guid IdUser { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? Password { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<FinanceDetail> FinanceDetails { get; set; } = new List<FinanceDetail>();

    public virtual ICollection<Friendship> FriendshipIdRefNavigations { get; set; } = new List<Friendship>();

    public virtual ICollection<Friendship> FriendshipIdUserNavigations { get; set; } = new List<Friendship>();

    public virtual ICollection<InvestmentFund> InvestmentFunds { get; set; } = new List<InvestmentFund>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Role? RoleNameNavigation { get; set; }

    public virtual ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
