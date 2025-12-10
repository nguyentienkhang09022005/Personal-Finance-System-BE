namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class EvaluateResponse
    {
        public Guid IdEvaluate { get; set; }

        public Guid? idUser { get; set; }

        public string? Name { get; set; }

        public string? UrlAvatar { get; set; }

        public int? Star { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreateAt { get; set; }
    }

    public class ListEvaluateResponse
    {
        public int? TotalComments { get; set; }

        public decimal? AverageStars { get; set; }

        public List<EvaluateResponse>? EvaluateResponses { get; set; }
    }
}
