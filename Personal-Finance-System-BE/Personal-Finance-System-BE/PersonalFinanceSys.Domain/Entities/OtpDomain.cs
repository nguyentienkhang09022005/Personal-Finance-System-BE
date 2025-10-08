namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class OtpDomain
    {
        public int IdOtp { get; set; }

        public OtpDomain(int idOtp)
        {
            if (idOtp < 6 || idOtp > 6)
            {
                throw new ArgumentException("OTP phải là 6 số!");
            }
            IdOtp = idOtp;
        }
    }
}
