using System.ComponentModel.DataAnnotations;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class AuthenticationRequest
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
