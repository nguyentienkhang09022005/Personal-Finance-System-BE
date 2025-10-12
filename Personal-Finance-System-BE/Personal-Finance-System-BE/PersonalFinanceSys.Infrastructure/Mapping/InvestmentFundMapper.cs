using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class InvestmentFundMapper : Profile
    {
        public InvestmentFundMapper() 
        {
            CreateMap<InvesmentFundDomain, InvestmentFund>().ReverseMap();

            CreateMap<InvestmentFundRequest, InvesmentFundDomain>();
        }
    }
}
