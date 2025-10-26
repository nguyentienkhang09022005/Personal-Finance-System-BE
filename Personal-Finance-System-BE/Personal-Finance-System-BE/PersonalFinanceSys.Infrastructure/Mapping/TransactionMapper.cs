using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            CreateMap<Transaction, TransactionDomain>();

            CreateMap<TransactionDomain, Transaction>()
                .ForMember(dest => dest.IdTransaction, opt => opt.Ignore());

            CreateMap<TransactionCreationRequest, TransactionDomain>()
                .ConstructUsing(src => new TransactionDomain(src.Amount, src.TransactionType));

            CreateMap<TransactionUpdateRequest, TransactionDomain>()
                .ConstructUsing(src => new TransactionDomain(src.Amount, src.TransactionType))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<TransactionDomain, TransactionResponse>();

            CreateMap<TransactionDomain, TransactionChartResponse>();
        }
    }
}
