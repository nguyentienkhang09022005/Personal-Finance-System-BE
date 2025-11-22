using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class FriendshipMapper : Profile
    {
        public FriendshipMapper()
        {
            CreateMap<Friendship, FriendshipDomain>();

            CreateMap<FriendshipDomain, Friendship>()
                .ForMember(dest => dest.IdFriendship, opt => opt.Ignore());

            CreateMap<EvaluateCreationRequest, FriendshipDomain>();

            CreateMap<EvaluateUpdateRequest, FriendshipDomain>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<FriendshipDomain, FriendshipResponse>();
        }
    }
}
