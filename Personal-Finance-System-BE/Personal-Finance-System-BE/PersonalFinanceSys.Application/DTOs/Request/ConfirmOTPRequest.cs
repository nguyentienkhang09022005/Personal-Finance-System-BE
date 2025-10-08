namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class ConfirmOTPRequest
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
    }
}
