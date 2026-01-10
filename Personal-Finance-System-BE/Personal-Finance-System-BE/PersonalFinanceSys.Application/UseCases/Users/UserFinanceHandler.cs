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

                decimal totalCollect = transactionResponses
                    .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeCollect)
                    .Sum(t => t.Amount);

                decimal totalExpense = transactionResponses
                    .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeExpense)
                    .Sum(t => t.Amount);

                decimal currentCashBalance = totalCollect - totalExpense;

                // Fund
                decimal cryptoValue = await _investmentDetailHandler.InvestmentAssetProfitByUserHandleAsync(idUser);

                // Tổng tài chính
                decimal totalNetWorth = currentCashBalance + cryptoValue;

                decimal positiveCash = currentCashBalance > 0 ? currentCashBalance : 0;
                decimal positiveCrypto = cryptoValue > 0 ? cryptoValue : 0;
                decimal totalPositiveAssets = positiveCash + positiveCrypto;

                var userFinanceResponse = new UserFinanceResponse
                {
                    totalAmount = totalNetWorth,
                    cash = currentCashBalance,
                    cashPercent = totalPositiveAssets == 0 ? 0 : (positiveCash / totalPositiveAssets) * 100,

                    crypto = cryptoValue,
                    cryptoPercent = totalPositiveAssets == 0 ? 0 : (positiveCrypto / totalPositiveAssets) * 100
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
