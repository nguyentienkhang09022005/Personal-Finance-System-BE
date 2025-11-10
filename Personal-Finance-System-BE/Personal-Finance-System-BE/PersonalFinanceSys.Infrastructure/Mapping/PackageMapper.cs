using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class PackageMapper : Profile
    {
        public PackageMapper() 
        {
            CreateMap<PackageDomain, Package>()
                        .ForMember(dest => dest.IdPackage, opt => opt.Ignore());

            CreateMap<Package, PackageDomain>()
                .ConstructUsing(src => new PackageDomain(src.Price, src.DurationDays))
                .ForMember(dest => dest.PermissionNames, opt => opt.MapFrom(src => src.PermissionNames));

            CreateMap<PackageCreationRequest, PackageDomain>()
                .ConstructUsing(src => new PackageDomain(src.Price, src.DurationDays))
                .ForMember(dest => dest.PermissionNames, opt => opt.Ignore());

            CreateMap<PackageUpdateRequest, PackageDomain>()
                .ConstructUsing(src => new PackageDomain(src.Price, src.DurationDays))
                .ForMember(dest => dest.PermissionNames, opt => opt.Ignore());

            CreateMap<PackageDomain, PackageResponse>();
        }
    }
}
