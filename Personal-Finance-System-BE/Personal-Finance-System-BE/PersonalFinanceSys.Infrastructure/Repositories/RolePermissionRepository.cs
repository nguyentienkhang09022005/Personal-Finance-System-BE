using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public RolePermissionRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddRolePermissionAsync(RolePermissionRequest rolePermissionRequest)
        {
            var role = await _context.Roles
                .Include(r => r.PermissionNames)
                .FirstOrDefaultAsync(r => r.RoleName == rolePermissionRequest.Role);

            if (role == null)
                throw new Exception("Không tìm thấy role!");
            

            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.PermissionName == rolePermissionRequest.Permission);

            if (permission == null)
                throw new Exception("Không tìm thấy permission!");

            if (!role.PermissionNames.Any(p => p.PermissionName == permission.PermissionName))
            {
                role.PermissionNames.Add(permission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RolePermissionResponse> ListRolePermissionResponse()
        {
            var roles = await _context.Roles
                .Include(r => r.PermissionNames)
                .ToListAsync();

            var roleResponses = _mapper.Map<List<ListRoleResponse>>(roles);

            return new RolePermissionResponse
            {
                Roles = roleResponses
            };
        }

        public async Task DeleteRolePermissionAsync(RolePermissionRequest rolePermissionRequest)
        {
            if (string.IsNullOrWhiteSpace(rolePermissionRequest.Role) || string.IsNullOrWhiteSpace(rolePermissionRequest.Permission))
                throw new ArgumentException("Vui lòng nhập đầy đủ role và permision!");

            var role = await _context.Roles
                .Include(r => r.PermissionNames)
                .FirstOrDefaultAsync(r => r.RoleName == rolePermissionRequest.Role);

            if (role == null)
                throw new Exception("Không tìm thấy role!");

            var permission = role.PermissionNames
                .FirstOrDefault(p => p.PermissionName == rolePermissionRequest.Permission);

            if (permission != null)
            {
                role.PermissionNames.Remove(permission);
                await _context.SaveChangesAsync();
            }
        }
    }
}
