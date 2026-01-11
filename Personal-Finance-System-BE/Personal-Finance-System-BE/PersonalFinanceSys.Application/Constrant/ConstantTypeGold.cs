namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant
{
    public class ConstantTypeGold
    {
        public static readonly Dictionary<string, string> SjcMapping = new Dictionary<string, string>
        {
            { "1l", "Vàng miếng 1 Lượng" },
            { "1c", "Vàng miếng 1 Chỉ" },
            { "5c", "Vàng miếng 5 Chỉ" },
            { "nhan1c", "Nhẫn SJC 1 Chỉ" },
            { "nutrang_9999", "Nữ trang 99.99%" },
            { "nutrang_99", "Nữ trang 99%" },
            { "nutrang_75", "Nữ trang 75%" }
        };

        public static readonly Dictionary<string, string> DojiLocationMapping = new Dictionary<string, string>
        {
            { "hn", "Hà Nội" },
            { "hcm", "TP. Hồ Chí Minh" },
            { "dn", "Đà Nẵng" },
            { "ct", "Cần Thơ" }
        };

        public static readonly Dictionary<string, string> PnjMapping = new Dictionary<string, string>
        {
            { "nhan_24k", "Nhẫn Trơn 24K" },
            { "nt_24k", "Nữ trang 24K" },
            { "nt_18k", "Nữ trang 18K" },
            { "nt_14k", "Nữ trang 14K" },
            { "nt_10k", "Nữ trang 10K" }
        };
    }
}
