namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class UserUpdateRequest
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? UrlAvatar { get; set; }
    }
}
