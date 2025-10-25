namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class UserDomain
    {
        public Guid IdUser { get; set; }

        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? Password { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsDarkmode { get; set; }

        public DateTime? CreateAt { get; set; }

        public UserDomain() { }

        public UserDomain(string email, string phone, string password)
        {
            SetEmail(email);
            SetPassword(password);
            SetPhone(phone);
        }

        // Phương thức kiểm tra tính hợp lệ của email
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Email không hợp lệ! Phải có dạng example@gmail.com!");
            }
            Email = email;
        }

        public void SetPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || phone.Length < 10 || phone.Length > 12)
            {
                throw new ArgumentException("Số điện thoại không hợp lệ!");
            }
            Phone = phone;
        }

        // Phương thức kiểm tra tính hợp lệ của password
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự!");
            }
            Password = password;
        }
    }
}
    