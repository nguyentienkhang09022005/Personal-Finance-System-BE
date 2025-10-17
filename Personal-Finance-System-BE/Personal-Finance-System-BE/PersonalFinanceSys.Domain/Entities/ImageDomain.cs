namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class ImageDomain
    {
        public Guid IdImage { get; set; }

        public string Url { get; set; } = null!;

        public DateTime? CreateAt { get; set; }

        public Guid? IdRef { get; set; }

        public string? RefType { get; set; }

        public ImageDomain() { }
    }
}
