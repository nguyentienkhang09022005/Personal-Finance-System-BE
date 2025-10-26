using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;

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

        public InvestmentDetailDomain(decimal price, decimal quantity, decimal fee, string type)
        {
            SetPrice(price);
            SetQuantity(quantity);
            SetFee(fee);
            SetType(type);
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Giá không được nhỏ hơn 0!");
            }
            Price = price;
        }

        public void SetQuantity(decimal quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0");
            }
            Quantity = quantity;
        }

        public void SetFee(decimal fee)
        {
            if (fee < 0)
            {
                throw new ArgumentException("Phí giao dịch phải lớn hơn 0");
            }
            Fee = fee;
        }

        public void SetType(string type)
        {
            if (type != ConstrantBuyAndSell.TypeBuy && type != ConstrantBuyAndSell.TypeSell)
            {
                throw new ArgumentException("Loại giao dịch không hợp lệ!");
            }
            Type = type;
        }
    }
}
