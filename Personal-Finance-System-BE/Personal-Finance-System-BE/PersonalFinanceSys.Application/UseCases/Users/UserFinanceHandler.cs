using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Users
{
    public class UserFinanceHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserFinanceHandler(ITransactionRepository transactionRepository, 
                                  IUserRepository userRepository,
                                  IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //public async Task<ApiResponse<UserFinanceResponse>> InfUserFinanceAsync(Guid idUser)
        //{
        //    try
        //    {
        //        var checkUserExist = await _userRepository.ExistUserAsync(idUser);
        //        if (!checkUserExist){
        //            return ApiResponse<UserFinanceResponse>.FailResponse("Không tìm thấy người dùng!", 404);
        //        }

        //        var transactions = await _transactionRepository.GetListTransactionAsync(idUser);
        //        var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);

        //        var expenseTransactions = transactionResponses // Lọc ra loại Thu
        //            .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeCollect)
        //            .ToList();

        //        decimal totalCollect = expenseTransactions == null ? 0 : expenseTransactions.Sum(t => t.Amount); // Tổng tiền nhận vào

                

        //        var userFinanceResponse = new UserFinanceResponse
        //        {
        //            totalAmount =,
        //            cash = totalCollect,
        //            cashPercent =,
        //            crypto=,
        //            cryptoPercent =,
        //        };

        //        return ApiResponse<TransactionSummaryResponse>.SuccessResponse("Lấy danh sách giao dịch thành công!", 200, summaryResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<TransactionSummaryResponse>.FailResponse(ex.Message, 500);
        //    }
        //}
    }
}
