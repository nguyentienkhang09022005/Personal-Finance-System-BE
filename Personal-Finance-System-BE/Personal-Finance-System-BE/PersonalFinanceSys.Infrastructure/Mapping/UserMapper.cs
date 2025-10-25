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
                .ConstructUsing(src => new UserDomain());

            // UserDomain -> User
            CreateMap<UserDomain, User>()
                .ForMember(dest => dest.IdUser, opt => opt.Ignore());

            // UserDomain -> UserResponse
            CreateMap<UserDomain, UserResponse>();

            // User -> UserResponse
            CreateMap<User, UserResponse>();

            // UserCreationRequest -> UserDomain
            CreateMap<UserCreationRequest, UserDomain>()
                .ConstructUsing(src => new UserDomain(src.Email, src.Phone, src.Password))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // UserUpdateRequest -> UserDomain
            CreateMap<UserUpdateRequest, UserDomain>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
