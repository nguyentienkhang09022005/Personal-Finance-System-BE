namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class RolePermissionResponse
    {
        public List<ListRoleResponse> Roles { get; set; } = new List<ListRoleResponse>();
    }

    public class ListRoleResponse
    {
        public string? RoleName { get; set; }

        public string? Description { get; set; }

        public List<PermissionResponse> Permissions { get; set; } = new List<PermissionResponse>();
    }
}
