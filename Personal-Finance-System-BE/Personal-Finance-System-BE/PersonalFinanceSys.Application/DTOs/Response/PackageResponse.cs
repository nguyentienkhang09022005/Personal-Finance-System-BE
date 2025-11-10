namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class PackageResponse
    {
        public Guid IdPackage { get; set; }

        public string? PackageName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public DateTime? CreateAt { get; set; }

        public List<PermissionResponse> Permissions { get; set; }
    }
}
