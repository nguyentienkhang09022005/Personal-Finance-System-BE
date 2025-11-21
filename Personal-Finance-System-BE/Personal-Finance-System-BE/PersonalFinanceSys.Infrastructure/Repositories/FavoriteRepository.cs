using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public FavoriteRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddFavoriteAsync(FavoriteDomain favoriteDomain)
        {
            var favorite = _mapper.Map<Favorite>(favoriteDomain);
            favorite.IdFavorite = Guid.NewGuid();

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFavoriteAsync(Guid idUser, Guid idPost)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.IdUser == idUser && f.IdPost == idPost)
                ?? throw new NotFoundException("Không tìm thấy mục yêu thích cần xóa!");

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistFavorite(Guid idUser, Guid idPost)
        {
            var exists = await _context.Favorites
    .AsNoTracking()
    .AnyAsync(f => f.IdPost == idPost && f.IdUser == idUser);

            Console.WriteLine($"Check favorite for user {idUser} post {idPost} => {exists}");
            return exists;

        }
    }
}
