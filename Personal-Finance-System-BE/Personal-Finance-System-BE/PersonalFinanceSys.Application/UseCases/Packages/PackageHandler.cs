using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Packages
{
    public class PackageHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;

        public PackageHandler(IPackageRepository packageRepository, IMapper mapper)
        {
            _packageRepository = packageRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ListPackageResponse>>> GetListPackageAsync(Guid idUser)
        {
            try
            {
                var (allPackages, boughtIds) = await _packageRepository.GetPackagesWithBoughtInfo(idUser);
                if (allPackages == null || !allPackages.Any()){
                    return ApiResponse<List<ListPackageResponse>>.SuccessResponse("Chưa có gói nào được tạo!", 200, new List<ListPackageResponse>());
                }

                var response = allPackages.Select(p => new ListPackageResponse
                {
                    IdPackage = p.IdPackage,
                    PackageName = p.PackageName,
                    Description = p.Description,
                    Price = p.Price,
                    DurationDays = p.DurationDays,
                    CreateAt = p.CreateAt,

                    Bought = boughtIds.Contains(p.IdPackage),

                    Permissions = p.PermissionNames.Select(per => new PermissionResponse
                    {
                        PermissionName = per.PermissionName,
                        Description = per.Description
                    }).ToList()
                }).ToList();
                return ApiResponse<List<ListPackageResponse>>.SuccessResponse("Lấy danh sách gói thành công!", 200, response);
            }
            catch (Exception ex){
                return ApiResponse<List<ListPackageResponse>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PackageResponse>> CreatePackageAsync(PackageCreationRequest packageCreationRequest)
        {
            try
            {
                var packageDomain = _mapper.Map<PackageDomain>(packageCreationRequest);
                var permissionNames = packageCreationRequest.PermissionNames ?? new List<string>();

                var packageCreated = await _packageRepository.AddPackageAsync(packageDomain, permissionNames);

                var packageResponse = _mapper.Map<PackageResponse>(packageCreated);
                packageResponse.Permissions = permissionNames
                    .Select(name => new PermissionResponse 
                    { 
                        PermissionName = name
                    })
                    .ToList();

                return ApiResponse<PackageResponse>.SuccessResponse("Tạo gói thành công!", 200, packageResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PackageResponse>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<string>> DeletePackageAsync(Guid idPackage)
        {
            try
            {
                await _packageRepository.DeletePackageAsync(idPackage);
                return ApiResponse<string>.SuccessResponse("Xóa gói thành công!", 200, string.Empty);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 404);
            }
        }

        public async Task<ApiResponse<PackageResponse>> UpdatePackageAsync(PackageUpdateRequest packageUpdateRequest, Guid idPackage)
        {
            try
            {
                var packageEntity = await _packageRepository.ExistPackage(idPackage);
                if (packageEntity == null){
                    return ApiResponse<PackageResponse>.FailResponse("Không tìm thấy gói!", 404);
                }
                var packageDomain = _mapper.Map<PackageDomain>(packageEntity);
                var permissionNames = packageUpdateRequest.PermissionNames ?? new List<string>();


                _mapper.Map(packageUpdateRequest, packageDomain);

                var packageUpdated = await _packageRepository.UpdatePackageAsync(packageDomain, packageEntity, permissionNames);

                var packageResponse = _mapper.Map<PackageResponse>(packageUpdated);
                packageResponse.Permissions = permissionNames
                    .Select(name => new PermissionResponse { PermissionName = name })
                    .ToList();

                return ApiResponse<PackageResponse>.SuccessResponse("Cập nhật thông tin gói thành công", 200, packageResponse);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<PackageResponse>.FailResponse(ex.Message, 404);
            }
        }
    }
}
