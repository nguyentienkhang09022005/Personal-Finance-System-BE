namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class CompareInvestmentDetailByMonthRequest
    {
        public int FirstMonth { get; set; }
        public int FirstYear { get; set; }

        public int SecondMonth { get; set; }
        public int SecondYear { get; set; }

        public Guid IdUser { get; set; }

        public Guid IdAsset { get; set; }
    }
}
