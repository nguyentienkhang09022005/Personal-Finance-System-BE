namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class GoldResponse
    {
        public List<GoldItemResponse>? SjcGold { get; set; }

        public List<GoldItemResponse>? DojiGold { get; set; }

        public List<GoldItemResponse>? PnjGold { get; set; }
    }

    public class GoldItemResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal SellPrice { get; set; }

        public string Location { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
