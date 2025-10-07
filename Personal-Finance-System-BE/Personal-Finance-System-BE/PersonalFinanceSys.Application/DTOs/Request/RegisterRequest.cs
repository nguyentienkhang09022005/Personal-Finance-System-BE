namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class RegisterRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
