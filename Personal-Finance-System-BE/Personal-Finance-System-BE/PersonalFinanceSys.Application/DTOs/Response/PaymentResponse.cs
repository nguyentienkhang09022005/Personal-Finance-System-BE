namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class PaymentResponse
    {
        public Guid IdPayment { get; set; }

        public decimal? Amount { get; set; }

        public string? Method { get; set; }

        public string? Status { get; set; }

        public string? OrderUrl { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}
