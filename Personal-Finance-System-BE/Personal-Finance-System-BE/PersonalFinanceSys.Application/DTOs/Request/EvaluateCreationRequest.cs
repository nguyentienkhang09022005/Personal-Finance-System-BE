namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class EvaluateCreationRequest
    {
        public int Star { get; set; }

        public string? Comment { get; set; }

        public Guid IdPost { get; set; }

        public Guid IdUser { get; set; }
    }
}
