namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class ForgotPasswordCacheData
    {
        public required string Otp { get; set; }
        public required string Email { get; set; }
    }
}
