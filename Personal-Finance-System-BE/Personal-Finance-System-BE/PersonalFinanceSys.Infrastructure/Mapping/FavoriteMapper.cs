using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class FavoriteMapper : Profile
    {
        public FavoriteMapper() 
        {
            CreateMap<Favorite, FavoriteDomain>();

            CreateMap<FavoriteDomain, Favorite>()
                .ForMember(dest => dest.IdFavorite, opt => opt.Ignore());

            CreateMap<FavoriteRequest, FavoriteDomain>();
        }
    }
}
