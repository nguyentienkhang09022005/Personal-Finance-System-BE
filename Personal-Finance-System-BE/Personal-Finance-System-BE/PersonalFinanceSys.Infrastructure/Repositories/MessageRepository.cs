using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MessageDomain> AddMessageAsync(MessageDomain messageDomain)
        {
            var friendship = await _context.Friendships
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.IdFriendship == messageDomain.IdFriendship)
                ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện!");

            var message = _mapper.Map<Message>(messageDomain);
            message.IdMessage = Guid.NewGuid();
            message.SendAt = DateTime.UtcNow;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return _mapper.Map<MessageDomain>(message);
        }

        public async Task DeleteMessageAsync(Guid idMessage)
        {
            var message = await _context.Messages
                .FindAsync(idMessage) ?? throw new NotFoundException("Không tìm thấy tin nhắn cần xóa!");

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistMessage(Guid idMessage)
        {
            return await _context.Messages
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(m => m.IdMessage == idMessage);
        }

        public async Task<Message> GetExistMessage(Guid idMessage)
        {
            var friendship = await _context.Messages
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(m => m.IdMessage == idMessage);

            return friendship ?? throw new NotFoundException("Không tìm thấy tin nhắn!");
        }

        public async Task<List<MessageDomain>> GetListMessageAsync(Guid idFriendship)
        {
            var messages = await _context.Messages
                .Where(m => m.IdFriendship == idFriendship)
                .AsNoTracking()
                .OrderBy(m => m.SendAt)
                .ToListAsync();
            return _mapper.Map<List<MessageDomain>>(messages);
        }
    }
}
