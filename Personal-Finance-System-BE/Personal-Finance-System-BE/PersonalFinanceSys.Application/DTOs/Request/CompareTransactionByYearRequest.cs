namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class CompareTransactionByYearRequest
    {
        public int Year1 { get; set; }

        public int Year2 { get; set; }

        public Guid IdUser { get; set; }
    }
}
