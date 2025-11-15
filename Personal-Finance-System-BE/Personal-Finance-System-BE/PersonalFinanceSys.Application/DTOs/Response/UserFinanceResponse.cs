namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class UserFinanceResponse
    {
        public decimal totalAmount { get; set; }

        public decimal cash { get; set; }

        public decimal cashPercent { get; set; }

        public decimal crypto { get; set; }

        public decimal cryptoPercent { get; set; }
    }
}
