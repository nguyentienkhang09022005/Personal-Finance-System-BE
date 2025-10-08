namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class ChangePasswordRequest
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
