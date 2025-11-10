using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class PackageDomain
    {
        public Guid IdPackage { get; set; }

        public string? PackageName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public DateTime? CreateAt { get; set; }

        public virtual ICollection<Permission> PermissionNames { get; set; } = new List<Permission>();

        public PackageDomain(decimal price, int durationDay) 
        { 
            SetPrice(price);
            SetDurationDays(durationDay);
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Giá không được nhỏ hơn 0!");
            }
            Price = price;
        }

        public void SetDurationDays(int durationDay)
        {
            if (durationDay < 0)
                throw new ArgumentException("Thời gian sử dụng không được nhỏ hơn 0!");

            DurationDays = durationDay;
        }
    }
}
