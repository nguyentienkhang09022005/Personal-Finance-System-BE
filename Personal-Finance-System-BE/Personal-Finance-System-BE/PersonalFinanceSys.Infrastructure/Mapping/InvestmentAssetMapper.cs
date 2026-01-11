using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class InvestmentAssetMapper : Profile
    {
        public InvestmentAssetMapper() 
        {
            // Domain -> Entity
            CreateMap<InvestmentAssetDomain, InvestmentAsset>()
                .ForMember(dest => dest.IdAsset, opt => opt.Ignore());

            // Entity -> Domain
            CreateMap<InvestmentAsset, InvestmentAssetDomain>();

            // Request -> Domain
            CreateMap<InvestmentAssetRequest, InvestmentAssetDomain>();

            CreateMap<InvestmentAssetGoldRequest, InvestmentAssetDomain>();
        }
    }
}
