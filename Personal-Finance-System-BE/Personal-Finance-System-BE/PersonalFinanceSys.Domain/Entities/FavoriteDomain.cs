namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class FavoriteDomain
    {
        public Guid IdFavorite { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdPost { get; set; }

        public Guid? IdUser { get; set; }

        public FavoriteDomain() { }
    }
}
