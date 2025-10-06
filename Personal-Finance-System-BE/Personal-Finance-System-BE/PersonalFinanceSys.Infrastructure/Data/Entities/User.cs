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

    public decimal? TotalAmount { get; set; }

    public bool? IsDarkmode { get; set; }

    public DateTime? CreateAt { get; set; }
}
