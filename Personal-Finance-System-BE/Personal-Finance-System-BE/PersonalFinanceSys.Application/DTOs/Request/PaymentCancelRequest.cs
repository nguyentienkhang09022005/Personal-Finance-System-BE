namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class PaymentCancelRequest
    {
        public Guid IdUser { get; set; }

        public Guid IdPackage { get; set; }
    }
}
