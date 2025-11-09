using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public PaymentRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentDomain paymentDomain)
        {
            Payment payment = _mapper.Map<Payment>(paymentDomain);
            payment.IdPayment = Guid.NewGuid();

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentDomain> GetPaymentByIdAppTransAsync(string IdAppTrans)
        {
            var payment = await _context.Payments
                            .IgnoreAutoIncludes()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.IdAppTrans == IdAppTrans);
            if (payment == null) return null;
            return _mapper.Map<PaymentDomain>(payment);
        }

        public Task UpdateStatusPaymentAsync(Guid idPayment, string status)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.IdPayment == idPayment);
            if (payment != null)
            {
                payment.Status = status;
                _context.Payments.Update(payment);
                return _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }
    }
}
