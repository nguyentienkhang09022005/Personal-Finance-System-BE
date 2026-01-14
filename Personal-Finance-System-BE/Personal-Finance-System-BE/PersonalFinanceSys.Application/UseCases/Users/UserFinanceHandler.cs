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

                // Transaction (Tính tiền mặt)
                var transactions = await _transactionRepository.GetListTransactionAsync(idUser);
                var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);

                decimal totalCollect = transactionResponses
                    .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeCollect)
                    .Sum(t => t.Amount);

                decimal totalExpense = transactionResponses
                    .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeExpense)
                    .Sum(t => t.Amount);

                decimal currentCashBalance = totalCollect - totalExpense;

                // Fund (Tính giá trị Crypto và Gold tách biệt)
                var (currentCryptoValue, currentGoldValue) = await _investmentDetailHandler.GetAssetAllocationByUserAsync(idUser);

                decimal totalNetWorth = currentCashBalance + currentCryptoValue + currentGoldValue;

                decimal positiveCash = currentCashBalance > 0 ? currentCashBalance : 0;
                decimal positiveCrypto = currentCryptoValue > 0 ? currentCryptoValue : 0;
                decimal positiveGold = currentGoldValue > 0 ? currentGoldValue : 0;

                decimal totalPositiveAssets = positiveCash + positiveCrypto + positiveGold;

                var userFinanceResponse = new UserFinanceResponse
                {
                    totalAmount = totalNetWorth,

                    // Cash
                    cash = currentCashBalance,
                    cashPercent = totalPositiveAssets == 0 ? 0 : Math.Round((positiveCash / totalPositiveAssets) * 100, 2),

                    // Crypto
                    crypto = currentCryptoValue,
                    cryptoPercent = totalPositiveAssets == 0 ? 0 : Math.Round((positiveCrypto / totalPositiveAssets) * 100, 2),

                    // Gold
                    gold = currentGoldValue,
                    goldPercent = totalPositiveAssets == 0 ? 0 : Math.Round((positiveGold / totalPositiveAssets) * 100, 2)
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
