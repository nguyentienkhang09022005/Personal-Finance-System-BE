using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class BudgetMapper : Profile
    {
        public BudgetMapper()
        {
            CreateMap<Budget, BudgetDomain>();

            CreateMap<BudgetDomain, Budget>()
                .ForMember(dest => dest.IdBudget, opt => opt.Ignore());

            CreateMap<BudgetCreationRequest, BudgetDomain>()
                .ConstructUsing(src => new BudgetDomain(src.BudgetGoal));

            CreateMap<BudgetUpdateRequest, BudgetDomain>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<BudgetDomain, BudgetResponse>();
        }
    }
}
