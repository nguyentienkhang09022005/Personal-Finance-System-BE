namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentAssetPostUpdateRequest
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? UrlImage { get; set; }

        public List<ListInvestmentAssetOfPost>? InvestmentAssetOfPost { get; set; }
    }
}
