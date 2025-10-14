using System;
using System.Collections.Generic;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

public partial class InvestmentAsset
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

    public virtual InvestmentFund? IdFundNavigation { get; set; }

    public virtual ICollection<InvestmentDetail> InvestmentDetails { get; set; } = new List<InvestmentDetail>();
}
