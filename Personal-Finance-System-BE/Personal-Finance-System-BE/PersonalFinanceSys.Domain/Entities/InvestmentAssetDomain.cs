using ServiceStack.Text;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class InvestmentAssetDomain
    {
        public Guid IdAsset { get; set; }

        public string? Id { get; set; }

        public string? AssetName { get; set; }

        public string? AssetSymbol { get; set; }

        public decimal? CurrentPrice { get; set; }

        public decimal? MarketCap { get; set; }

        public decimal? TotalVolume { get; set; }

        public decimal? PriceChangePercentage24h { get; set; }

        public Guid? IdFund { get; set; }

        public InvestmentAssetDomain(decimal CurrentPrice, decimal MarketCap, decimal TotalVolume) 
        {
            SetCurrentPrice(CurrentPrice);
            SetMarketCap(MarketCap);
            SetTotalVolume(TotalVolume);
        }

        public void SetCurrentPrice(decimal currentPrice)
        {
            if (currentPrice < 0)
            {
                throw new ArgumentException("Giá hiện tại không được < 0!");
            }
            CurrentPrice = currentPrice;
        }

        public void SetMarketCap(decimal marketCap)
        {
            if (marketCap < 0)
            {
                throw new ArgumentException("MarketCap không được < 0!");
            }
            MarketCap = marketCap;
        }

        public void SetTotalVolume(decimal totalVolume)
        {
            if (totalVolume < 0)
            {
                throw new ArgumentException("TotalVolume không được < 0!");
            }
            TotalVolume = totalVolume;
        }
    }
}
