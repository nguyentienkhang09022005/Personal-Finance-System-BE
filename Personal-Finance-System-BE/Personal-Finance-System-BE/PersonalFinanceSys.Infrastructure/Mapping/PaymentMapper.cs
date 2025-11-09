using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class PaymentMapper : Profile
    {
        public PaymentMapper()
        {
            CreateMap<PaymentRequest, Payment>();

            CreateMap<PaymentRequest, PaymentDomain>();

            CreateMap<Payment, PaymentResponse>();

            CreateMap<Payment, PaymentDomain>();

            CreateMap<PaymentDomain, Payment>();
        }
    }
}
