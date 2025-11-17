using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class PostResponse
    {
        public Guid IdPost { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public bool? IsApproved { get; set; }

        public string? UrlImage { get; set; }

        public List<SnapshotResponse>? SnapshotResponse { get; set; }

        public UserOfPostResponse? UserOfPostResponse { get; set; }
    }

    public class UserOfPostResponse
    {
        public Guid IdUser { get; set; }

        public string? Name { get; set; }

        public string? UrlAvatar { get; set; }
    }

    public class SnapshotResponse
    {
        // Transaction
        public List<TransactionOfPost>? TransactionOfPosts { get; set; }

        // Asset
        public List<ListInvestmentAssetOfPost>? InvestmentAssetOfPosts { get; set; }
    }
}
