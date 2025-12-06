using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public FriendshipRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AcceptFriendshipAsync(FriendshipDomain friendshipDomain, Friendship friendship)
        {
            _mapper.Map(friendshipDomain, friendship);
            friendship.UpdateAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task AddFriendshipAsync(FriendshipDomain friendshipDomain)
        {
            var friendship = _mapper.Map<Friendship>(friendshipDomain);
            friendship.IdFriendship = Guid.NewGuid();
            friendship.Status = ConstantStatusFriendship.FriendshipPending;
            friendship.CreateAt = DateTime.UtcNow;
            friendship.UpdateAt = null;

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFriendshipAsync(Guid idFriendship)
        {
            var friendship = await _context.Friendships
                .FindAsync(idFriendship) ?? throw new NotFoundException("Không tìm thấy bạn bè cần xóa!");

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistFriendship(Guid idFriendship)
        {
            return await _context.Friendships
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(f => f.IdFriendship == idFriendship);
        }

        public async Task<Friendship> GetExistFriendship(Guid idFriendship)
        {
            var friendship = await _context.Friendships
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(f => f.IdFriendship == idFriendship);

            return friendship ?? throw new NotFoundException("Không tìm thấy bạn bè!");
        }

        public async Task<bool> ExistFriendship(Guid idRef, Guid idUser)
        {
            return await _context.Friendships
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(f => f.IdRef == idRef && f.IdUser == idUser);
        }

        // Lấy danh sách bạn bè của user
        public async Task<List<FriendshipDomain>> GetListFriendshipOfUserAsync(Guid idUser)
        {
            var friendships = await _context.Friendships
                .Where(f => (f.IdUser == idUser || f.IdRef == idUser)
                    && f.Status == ConstantStatusFriendship.FriendshipAccept).AsNoTracking()
                .Include(f => f.IdUserNavigation)
                .Include(f => f.IdRefNavigation)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<FriendshipDomain>>(friendships);
        }

        // Lấy danh sách lời mời kết bạn đã gửi với trạng thái đang chờ
        public async Task<List<FriendshipDomain>> GetListFriendshipSentWithStatusPendingByUserAsync(Guid idUser)
        {
            var friendships = await _context.Friendships
                .Where(f => f.IdUser == idUser && f.Status == ConstantStatusFriendship.FriendshipPending)
                .Include(f => f.IdUserNavigation)
                .Include(f => f.IdRefNavigation)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<FriendshipDomain>>(friendships);
        }

        // Lấy danh sách lời mời kết bạn đã nhận với trạng thái đang chờ
        public async Task<List<FriendshipDomain>> GetListFriendshipReceivedWithStatusPendingByUserAsync(Guid idUser)
        {
            var friendships = await _context.Friendships
                .Where(f => f.IdRef == idUser && f.Status == ConstantStatusFriendship.FriendshipPending)
                .Include(f => f.IdUserNavigation)
                .Include(f => f.IdRefNavigation)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<FriendshipDomain>>(friendships);
        }

        public async Task RejectFriendshipAsync(FriendshipDomain friendshipDomain, Friendship friendship)
        {
            _mapper.Map(friendshipDomain, friendship);
            friendship.UpdateAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
