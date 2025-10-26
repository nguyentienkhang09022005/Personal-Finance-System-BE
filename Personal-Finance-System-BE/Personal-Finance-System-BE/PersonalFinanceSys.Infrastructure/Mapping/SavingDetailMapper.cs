using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class SavingDetailMapper : Profile
    {
        public SavingDetailMapper()
        {
            CreateMap<SavingDetail, SavingDetailDomain>();

            CreateMap<SavingDetailDomain, SavingDetail>()
                .ForMember(dest => dest.IdDetail, opt => opt.Ignore());

            CreateMap<SavingDetailRequest, SavingDetailDomain>()
                .ConstructUsing(src => new SavingDetailDomain(src.Amount));

            CreateMap<SavingDetailDomain, SavingDetailResponse>();
        }
    }
}
