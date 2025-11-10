namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class PackageCreationRequest
    {
        public string? PackageName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public List<string> PermissionNames { get; set; } = new();
    }
}
