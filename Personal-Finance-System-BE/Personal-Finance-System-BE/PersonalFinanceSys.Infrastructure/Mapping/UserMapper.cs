using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // User -> UserDomain
            CreateMap<User, UserDomain>()
                .ConstructUsing(u => new UserDomain(u.Name, u.Email, u.Password)
                {
                    IdUser = u.IdUser
                });

            // UserDomain -> User
            CreateMap<UserDomain, User>().ReverseMap();

            // UserDomain -> UserResponse
            CreateMap<UserDomain, UserResponse>();

            // User -> UserResponse
            CreateMap<User, UserResponse>();

            // UserRequest -> UserDomain
            CreateMap<UserRequest, UserDomain>();
        }
    }
}
