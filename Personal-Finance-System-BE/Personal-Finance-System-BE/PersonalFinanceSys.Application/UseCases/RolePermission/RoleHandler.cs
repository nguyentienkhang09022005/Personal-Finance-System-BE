using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.RolePermission
{
    public class RoleHandler
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<RoleResponse>>> GetAllRolesAsync()
        {
            var roleDomains = await _roleRepository.GetListRoleAsync();
            var roleResposnes = _mapper.Map<List<RoleResponse>>(roleDomains);
            return ApiResponse<List<RoleResponse>>.SuccessResponse("Lấy danh sách quyền thành công!", 200, roleResposnes);
        }
    }
}
