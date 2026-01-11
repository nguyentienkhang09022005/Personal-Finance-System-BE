namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentAssetGoldRequest
    {
        public required string Id { get; set; }

        public required string AssetName { get; set; }

        public required string MappingKey { get; set; }

        public Guid IdFund { get; set; }
    }
}
