using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class SavingGoalMapper : Profile
    
        {
        public SavingGoalMapper()
        {
            CreateMap<SavingGoal, SavingGoalDomain>();

            CreateMap<SavingGoalDomain, SavingGoal>()
                .ForMember(dest => dest.IdSaving, opt => opt.Ignore());

            CreateMap<SavingGoalCreationRequest, SavingGoalDomain>()
                .ConstructUsing(src => new SavingGoalDomain(src.TargetAmount, src.TargetDate));

            CreateMap<SavingGoalUpdateRequest, SavingGoalDomain>()
                .ConstructUsing(src => new SavingGoalDomain(src.TargetDate))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<SavingGoalDomain, SavingGoalResponse>();
        }
    }
}
