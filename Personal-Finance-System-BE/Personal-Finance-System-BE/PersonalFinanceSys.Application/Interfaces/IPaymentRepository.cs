using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<PaymentResponse> CreatePaymentAsync(PaymentDomain paymentDomain);

        Task<PaymentDomain> GetPaymentByIdAppTransAsync(string IdAppTrans);

        Task<PaymentDomain> GetPaymentByUserIdAndPackageIdAsync(Guid idUser, Guid idPackage);

        Task<bool> CheckExistPaymentWithStatusSuccess(Guid idUser, Guid idPackage);

        Task UpdateStatusPaymentAsync(Guid idPayment, string status);
    }
}
