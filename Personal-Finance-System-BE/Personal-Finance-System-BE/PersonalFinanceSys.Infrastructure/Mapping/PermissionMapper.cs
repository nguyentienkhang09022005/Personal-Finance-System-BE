using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper() 
        {
            CreateMap<Permission, PermissionDomain>();

            CreateMap<PermissionDomain, PermissionResponse>();

            CreateMap<Permission, PermissionResponse>();
        }
    }
}
