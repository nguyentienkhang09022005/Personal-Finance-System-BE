using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvalidatedTokenRepository
    {
        Task<bool> ExistByIdAsync(Guid idToken);
        Task AddInvalidatedTokenAsync(InvalidatedTokenDomain invalidatedTokenDomain);
    }
}
