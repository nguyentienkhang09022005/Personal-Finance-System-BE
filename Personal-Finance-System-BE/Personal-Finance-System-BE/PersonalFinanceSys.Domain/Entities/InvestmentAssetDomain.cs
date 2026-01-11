using ServiceStack.Text;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class InvestmentAssetDomain
    {
        public Guid IdAsset { get; set; }

        public string? Id { get; set; }

        public string? AssetName { get; set; }

        public string? AssetSymbol { get; set; }

        public Guid? IdFund { get; set; }

        public string? AssetType { get; set; }

        public string? MappingKey { get; set; }

        public InvestmentAssetDomain() {}
    }
}
