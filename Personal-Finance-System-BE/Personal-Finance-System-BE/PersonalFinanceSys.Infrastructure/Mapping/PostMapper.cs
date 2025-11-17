using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class PostMapper : Profile
    {
        public PostMapper() 
        {
            CreateMap<Post, PostDomain>();

            CreateMap<PostDomain, Post>();

            CreateMap<PostDomain, PostResponse>()
                 .ForMember(dest => dest.UserOfPostResponse,
                           opt => opt.MapFrom(src => src.IdUserNavigation))
                 .ForMember(dest => dest.SnapshotResponse, opt => opt.Ignore());

            CreateMap<TransactionPostCreationRequest, PostDomain>()
                .ForMember(dest => dest.Snapshot, opt => opt.Ignore());

            CreateMap<InvestmentAssetPostCreationRequest, PostDomain>()
                .ForMember(dest => dest.Snapshot, opt => opt.Ignore());

            CreateMap<ApprovePostRequest, PostDomain>();

            CreateMap<TransactionPostUpdateRequest, PostDomain>();

            CreateMap<InvestmentAssetPostUpdateRequest, PostDomain>();

            CreateMap<PostUpdateRequest, PostDomain>();
        }
    }
}
