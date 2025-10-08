namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class InvalidatedToken
{
    public Guid IdToken { get; set; }

    public DateTime? ExpiryTime { get; set; }
}
