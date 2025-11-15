using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

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
        public async Task<UserDomain> AddUserAsync(UserDomain users)
        {
            var user = _mapper.Map<User>(users);
            user.IdUser = Guid.NewGuid();
            user.RoleName = ConstantRole.UserRole;

            if (!string.IsNullOrEmpty(user.Password)){
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDomain>(user);
        }

        // Delete User
        public async Task DeleteUserAsync(Guid idUser)
        {
            var user = await _context.Users.FindAsync(idUser) ?? throw new NotFoundException("Không tìm thấy người dùng!");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // Check Exist User
        public async Task<User> GetExistUserAsync(Guid idUser)
        {
            var user = await _context.Users
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            return user ?? throw new NotFoundException("Không tìm người dùng!");
        }

        public async Task<bool> ExistUserAsync(Guid idUser)
        {
            return await _context.Users
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(u => u.IdUser == idUser);
        }

        // Get List User
        public async Task<List<UserDomain?>> GetListUserAsync()
        { 
            var users = await _context.Users
                .IgnoreAutoIncludes()
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<UserDomain?>>(users);
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
            var user = await _context.Users
                    .AsNoTracking()
                    .IgnoreAutoIncludes()
                    .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                throw new NotFoundException("Không tìm thấy thông tin người dùng!");
            return _mapper.Map<UserDomain>(user);
        }

        // Update User
        public async Task<UserDomain?> UpdateUserAsync(UserDomain userDomain, User user)
        {
            _mapper.Map(userDomain, user);

            await _context.SaveChangesAsync();
            return _mapper.Map<UserDomain>(user);
        }
    }
}
