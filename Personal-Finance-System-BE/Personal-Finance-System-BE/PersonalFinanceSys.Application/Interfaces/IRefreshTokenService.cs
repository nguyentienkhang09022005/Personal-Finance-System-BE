namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task saveRefreshToken(string idUser, string refreshToken, TimeSpan duration);
        Task<string?> getRefreshToken(string idUser);
        Task deleteRefreshToken(string idUser);
    }
}
