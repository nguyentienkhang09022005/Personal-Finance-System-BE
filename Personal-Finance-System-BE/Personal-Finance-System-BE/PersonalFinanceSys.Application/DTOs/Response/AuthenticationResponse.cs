namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class AuthenticationResponse
    {
        public string? Token { get; set; }

        public UserResponse? InfUser { get; set; }
    }
}
