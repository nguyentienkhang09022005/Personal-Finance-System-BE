namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class InvestmentAssetRequest
    {
        public required string Id { get; set; }

        public required string AssetName { get; set; }

        public string AssetSymbol { get; set; }

        public Guid IdFund { get; set; }
    }
}
