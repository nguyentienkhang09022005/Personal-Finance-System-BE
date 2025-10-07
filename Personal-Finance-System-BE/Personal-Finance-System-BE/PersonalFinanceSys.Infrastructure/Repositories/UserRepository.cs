using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PersonFinanceSysDbContext _context;

        public UserRepository(PersonFinanceSysDbContext context)
        {
            _context = context;
        }

        // Add User
        public async Task AddUserAsync(UserDomain users)
        {
            var user = Mapping.UserMapper.toUserDB(users);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public Task DeleteUserAsync(Guid idUser)
        {
            throw new NotImplementedException();
        }

        // Get User By Email
        public async Task<UserDomain?> GetUserByEmailAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return null;
            return Mapping.UserMapper.toUserDomain(user);
        }

        // Get User By Id
        public async Task<UserDomain?> GetUserByIdAsync(Guid idUser)
        {
            var user = await _context.Users.FindAsync(idUser);
            if (user == null) return null;
            return Mapping.UserMapper.toUserDomain(user);
        }
    }
}
