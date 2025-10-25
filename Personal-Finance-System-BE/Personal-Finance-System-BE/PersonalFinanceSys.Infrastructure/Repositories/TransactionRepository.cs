using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionDomain> AddTransactionAsync(TransactionDomain transactionDomain)
        {
            var transaction = _mapper.Map<Transaction>(transactionDomain);
            transaction.IdTransaction = Guid.NewGuid();

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionDomain>(transaction);
        }

        public async Task DeleteTransactionAsync(Guid idTransaction)
        {
            var transaction = await _context.Transactions.FindAsync(idTransaction)
                ?? throw new NotFoundException("Không tìm giao dịch cần xóa!");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TransactionDomain>> GetListTransactionAsync(Guid idUser)
        {
            var transactions = _context.Transactions
                .Where(t => t.IdUser == idUser)
                .IgnoreAutoIncludes()
                .AsNoTracking()
                .ToList();
            return _mapper.Map<List<TransactionDomain>>(transactions);
        }

        public async Task<TransactionDomain> GetTransactionByIdAsync(Guid idTransaction)
        {
            var transaction = _context.Transactions
                .FirstOrDefault(t => t.IdTransaction == idTransaction);

            if (transaction == null)
                throw new NotFoundException("Không tìm thấy thông tin của giao dịch!");
            return _mapper.Map<TransactionDomain>(transaction);
        }

        public async Task<TransactionDomain> UpdateTransactionAsync(TransactionDomain transactionDomain, Transaction transaction)
        {
            _mapper.Map(transactionDomain, transaction);

            await _context.SaveChangesAsync();
            return _mapper.Map<TransactionDomain>(transaction);
        }

        public async Task<Transaction> ExistTransaction(Guid idTransaction)
        {
            var transaction = await _context.Transactions
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(t => t.IdTransaction == idTransaction);

            return transaction ?? throw new NotFoundException("Không tìm giao dịch!");
        }
    }
}
