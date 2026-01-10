namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class GoldResponse
    {
        public List<SjcGoldItemResponse>? SjcGold { get; set; }

        public List<DojiGoldItemResponse>? DojiGold { get; set; }

        public List<PnjGoldItemResponse>? PnjGold { get; set; }
    }
}
