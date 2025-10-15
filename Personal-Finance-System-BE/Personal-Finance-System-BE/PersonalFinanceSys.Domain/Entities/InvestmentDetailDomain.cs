namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class InvestmentDetailDomain
    {
        public Guid IdDetail { get; set; }

        public string? Type { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public decimal Expense { get; set; }

        public decimal Fee { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdAsset { get; set; }

        public InvestmentDetailDomain(decimal Price, decimal Quantity, decimal fee)
        {
            SetPrice(Price);
            SetQuantity(Quantity);
            SetFee(fee);
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("giá không được < 0!");
            }
            Price = price;
        }

        public void SetQuantity(decimal quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Số lượng phải > 0");
            }
            Quantity = quantity;
        }

        public void SetFee(decimal fee)
        {
            if (fee < 0)
            {
                throw new ArgumentException("Phí giao dịch phải > 0");
            }
            Fee = fee;
        }
    }
}
