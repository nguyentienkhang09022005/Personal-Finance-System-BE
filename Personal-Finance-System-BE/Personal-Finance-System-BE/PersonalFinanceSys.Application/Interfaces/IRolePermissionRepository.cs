using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<RolePermissionResponse> ListRolePermissionResponse();

        Task AddRolePermissionAsync(RolePermissionRequest rolePermissionRequest);

        Task DeleteRolePermissionAsync(RolePermissionRequest rolePermissionRequest);

        Task<List<string>> GetPermissionNamesByRoleAsync(string roleName);
    }
}
