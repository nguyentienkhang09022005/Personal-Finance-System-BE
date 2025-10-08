namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class RegisterCacheData
    {
        public required string Otp { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
