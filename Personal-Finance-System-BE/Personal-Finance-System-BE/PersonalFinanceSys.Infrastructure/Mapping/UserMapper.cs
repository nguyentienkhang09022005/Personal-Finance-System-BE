using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDomain>()
                .ConstructUsing(u => new UserDomain(u.Name, u.Email, u.Password)
                {
                    IdUser = u.IdUser
                });

            CreateMap<UserDomain, User>().ReverseMap();

            CreateMap<UserDomain, UserResponse>();

            CreateMap<User, UserResponse>();
        }
    }
}
