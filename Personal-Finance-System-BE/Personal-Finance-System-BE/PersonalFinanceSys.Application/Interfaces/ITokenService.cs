using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> generateAccessToken(UserDomain user);
        Task<string> generateRefreshToken(UserDomain user);
    }
}
