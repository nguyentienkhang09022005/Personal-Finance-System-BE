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

            CreateMap<FriendshipCreationRequest, FriendshipDomain>();

            CreateMap<FriendshipUpdateRequest, FriendshipDomain>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<FriendshipDomain, InfFriendshipResponse>()
                    .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.IdUserNavigation))
                    .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.IdRefNavigation));

            CreateMap<FriendshipDomain, FriendshipResponse>()
                    .ForMember(dest => dest.infFriendshipResponse,
                               opt => opt.MapFrom(src => src));
        }
    }
}
