namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class ZaloPayCallbackData
    {
        public int AppId { get; set; }

        public string AppTransId { get; set; }

        public long AppTime { get; set; }

        public string AppUser { get; set; }

        public long Amount { get; set; }
    }
}
