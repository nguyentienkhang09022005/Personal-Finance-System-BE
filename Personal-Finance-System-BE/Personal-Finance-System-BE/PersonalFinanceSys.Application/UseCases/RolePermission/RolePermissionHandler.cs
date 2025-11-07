using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.RolePermission
{
    public class RolePermissionHandler
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;

        public RolePermissionHandler(IRolePermissionRepository rolePermissionRepository, IMapper mapper)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> AddRolePermissionHandleAsync(RolePermissionRequest rolePermissionRequest)
        {
            try
            {
                await _rolePermissionRepository.AddRolePermissionAsync(rolePermissionRequest);
                return ApiResponse<string>.SuccessResponse("Phân quyền thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse($"Lỗi hệ thống: {ex}", 400);
            }
        }

        public async Task<ApiResponse<RolePermissionResponse>> ListRolePermissionHandleAsync()
        {
            try
            {
                RolePermissionResponse rolePermissionResponse = await _rolePermissionRepository.ListRolePermissionResponse();
                return ApiResponse<RolePermissionResponse>.SuccessResponse("Phân quyền thành công!", 200, rolePermissionResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<RolePermissionResponse>.FailResponse($"Lỗi hệ thống: {ex}", 400);
            }
        }

        public async Task<ApiResponse<string>> RemoveRolePermissionHandleAsync(RolePermissionRequest rolePermissionRequest)
        {
            try
            {
                await _rolePermissionRepository.DeleteRolePermissionAsync(rolePermissionRequest);
                return ApiResponse<string>.SuccessResponse("Xóa phân quyền thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse($"Lỗi hệ thống: {ex}", 400);
            }
        }
    }
}
