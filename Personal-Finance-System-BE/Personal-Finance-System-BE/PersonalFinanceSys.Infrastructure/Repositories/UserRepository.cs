using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Add User
        public async Task AddUserAsync(UserDomain users)
        {
            var user = _mapper.Map<User>(users);
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
            var user = await _context.Users
                .IgnoreAutoIncludes()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            return _mapper.Map<UserDomain>(user);
        }

        // Get User By Id
        public async Task<UserDomain?> GetUserByIdAsync(Guid idUser)
        {
            var user = await _context.Users.FindAsync(idUser);
            if (user == null) return null;
            return _mapper.Map<UserDomain>(user);
        }
    }
}
