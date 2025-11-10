namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class PaymentRequest
    {
        public Guid IdUser { get; set; }

        public Guid IdPackage { get; set; }

        public decimal Amount { get; set; }
    }
}
