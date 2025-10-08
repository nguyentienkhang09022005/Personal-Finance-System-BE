namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class Otp
{
    public int IdOtp { get; set; }

    public Guid? IdUser { get; set; }

    public DateTime? ExpiryTime { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
