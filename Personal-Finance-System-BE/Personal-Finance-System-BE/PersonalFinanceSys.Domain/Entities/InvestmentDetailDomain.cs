namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class InvestmentDetailDomain
    {
        public Guid IdDetail { get; set; }

        public string? Type { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public decimal? Expense { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdAsset { get; set; }

        public InvestmentDetailDomain(decimal Price, int Quantity) 
        {
            SetPrice(Price);
            SetQuantity(Quantity);
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("giá không được < 0!");
            }
            Price = price;
        }

        public void SetQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Số lượng phải > 0");
            }
            Quantity = quantity;
        }
    }
}
