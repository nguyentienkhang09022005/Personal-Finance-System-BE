using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class EvaluateMapper : Profile
    {
        public EvaluateMapper() 
        {
            CreateMap<Evaluate, EvaluateDomain>();

            CreateMap<EvaluateDomain, Evaluate>()
                .ForMember(dest => dest.IdEvaluate, opt => opt.Ignore());

            CreateMap<EvaluateCreationRequest, EvaluateDomain>()
                .ConstructUsing(src => new EvaluateDomain(src.Star));

            CreateMap<EvaluateUpdateRequest, EvaluateDomain>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<EvaluateDomain, EvaluateResponse>();
        }
    }
}
