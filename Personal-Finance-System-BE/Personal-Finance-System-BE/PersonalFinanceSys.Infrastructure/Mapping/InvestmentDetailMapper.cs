using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class InvestmentDetailMapper : Profile
    {
        public InvestmentDetailMapper() 
        {
            CreateMap<InvestmentDetailDomain, InvestmentDetail>()
                .ForMember(dest => dest.IdDetail, opt => opt.Ignore());

            // Entity -> Domain
            CreateMap<InvestmentDetail, InvestmentDetailDomain>();

            // Request -> Domain
            CreateMap<InvestmentDetailRequest, InvestmentDetailDomain>()
                .ConstructUsing(src => new InvestmentDetailDomain(src.Price, src.Quantity, src.Fee, src.Type));
        }
    }
}
