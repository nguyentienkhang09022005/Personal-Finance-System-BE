using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteAsync(FavoriteDomain favoriteDomain);

        Task DeleteFavoriteAsync(Guid idUser, Guid idPost);
    }
}
