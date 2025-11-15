using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Users
{
    public class UserFinanceHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly InvestmentDetailHandler _investmentDetailHandler;
        private readonly IMapper _mapper;

        public UserFinanceHandler(ITransactionRepository transactionRepository, 
                                  IUserRepository userRepository,
                                  InvestmentDetailHandler investmentDetailHandler,
                                  IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _investmentDetailHandler = investmentDetailHandler;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserFinanceResponse>> InfUserFinanceAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist)
                {
                    return ApiResponse<UserFinanceResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                // Transaction
                var transactions = await _transactionRepository.GetListTransactionAsync(idUser);
                var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);

                var expenseTransactions = transactionResponses // Lọc ra loại Thu
                    .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeCollect)
                    .ToList();

                decimal totalCollect = expenseTransactions == null ? 0 : expenseTransactions.Sum(t => t.Amount); // Tổng tiền nhận vào

                // Fund
                decimal profitResponse = await _investmentDetailHandler.InvestmentAssetProfitByUserHandleAsync(idUser);

                // Tổng tài chính
                decimal totalAmount = totalCollect + profitResponse;

                var userFinanceResponse = new UserFinanceResponse
                {
                    totalAmount = totalAmount,
                    cash = totalCollect,
                    cashPercent = totalAmount == 0 ? 0 : (totalCollect / totalAmount) * 100,
                    crypto = profitResponse,
                    cryptoPercent = totalAmount == 0 ? 0 : (profitResponse / totalAmount) * 100
                };

                return ApiResponse<UserFinanceResponse>.SuccessResponse("Lấy danh sách tài chính thành công!", 200, userFinanceResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserFinanceResponse>.FailResponse(ex.Message, 500);
            }
        }
    }
}
