namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class InvalidatedTokenDomain
    {
        public Guid IdToken { get; set; }

        public DateTime? ExpiryTime { get; set; }

        public InvalidatedTokenDomain(Guid idToken, DateTime expiryTime) 
        {
            IdToken = idToken;
            ExpiryTime = expiryTime;
        }
    }
}
