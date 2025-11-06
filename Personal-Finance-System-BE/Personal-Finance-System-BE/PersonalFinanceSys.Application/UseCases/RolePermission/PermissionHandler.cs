using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.RolePermission
{
    public class PermissionHandler
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionHandler(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<PermissionResposne>>> GetAllPermissionsAsync()
        {
            var permissionDomains = await _permissionRepository.GetListPermissionAsync();
            var permissionResponses = _mapper.Map<List<PermissionResposne>>(permissionDomains);
            return ApiResponse<List<PermissionResposne>>.SuccessResponse("Lấy danh chức năng của quyền thành công!", 200, permissionResponses);
        }
    }
}
