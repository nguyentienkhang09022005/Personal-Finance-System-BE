using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class InvestmentFundMapper : Profile
    {
        public InvestmentFundMapper() 
        {
            // Domain <-> entity
            CreateMap<InvestmentFundDomain, InvestmentFund>()
                        .ForMember(dest => dest.IdFund, opt => opt.Ignore());

            CreateMap<InvestmentFund, InvestmentFundDomain>();

            // Request -> Domain
            CreateMap<InvestmentFundCreationRequest, InvestmentFundDomain>();
            CreateMap<InvestmentFundUpdateRequest, InvestmentFundDomain>();

            // Domain -> Response
            CreateMap<InvestmentFundDomain, InvestmentFundResponse>();
        }
    }
}
