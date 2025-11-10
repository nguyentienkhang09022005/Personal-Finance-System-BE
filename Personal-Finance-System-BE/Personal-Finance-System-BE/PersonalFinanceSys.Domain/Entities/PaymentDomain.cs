namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class PaymentDomain
    {
        public Guid IdPayment { get; set; }

        public decimal? Amount { get; set; }

        public string? Method { get; set; }

        public string? Status { get; set; }

        public string? IdAppTrans { get; set; }

        public int? IdZpTrans { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdUser { get; set; }

        public Guid? IdPackage { get; set; }

        public PaymentDomain(){}
    }
}
