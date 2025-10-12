using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserDomain users);
        Task<List<UserDomain?>> GetListUserAsync();
        Task<UserDomain?> GetUserByIdAsync(Guid idUser);
        Task<UserDomain?> GetUserByEmailAsync(string email);
        Task DeleteUserAsync(Guid idUser);
        Task<UserDomain?> UpdateUserAsync(UserDomain userDomain);
    }
}
