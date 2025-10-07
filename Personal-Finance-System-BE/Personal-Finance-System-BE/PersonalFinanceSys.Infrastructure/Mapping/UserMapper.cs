using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public static class UserMapper
    {
        public static UserDomain toUserDomain(User user) 
            => new UserDomain(user.Name, user.Email, user.Password);

        public static User toUserDB(UserDomain users) 
            => new User{IdUser = users.IdUser, Name = users.Name, Email = users.Email, Password = users.Password};
    }
}
