using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Transactions
{
    public class TransactionHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TransactionHandler(ITransactionRepository transactionRepository, 
                                  IUserRepository userRepository,
                                  IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateTransactionAsync(TransactionCreationRequest transactionCreationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(transactionCreationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người dùng!", 404);

                var transactionDomain = _mapper.Map<TransactionDomain>(transactionCreationRequest);
                await _transactionRepository.AddTransactionAsync(transactionDomain);
                return ApiResponse<string>.SuccessResponse("Tạo giao dịch thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteTransactionAsync(Guid idTransaction)
        {
            try
            {
                await _transactionRepository.DeleteTransactionAsync(idTransaction);
                return ApiResponse<string>.SuccessResponse("Xóa giao dịch thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<TransactionResponse>> UpdateTransactionAsync(Guid idTransaction, TransactionUpdateRequest transactionUpdateRequest)
        {
            try
            {
                var transactionEntity = await _transactionRepository.ExistTransaction(idTransaction);
                if (transactionEntity == null)
                {
                    return ApiResponse<TransactionResponse>.FailResponse("Không tìm thấy giao dịch!", 404);
                }
                var transactionDomain = _mapper.Map<TransactionDomain>(transactionEntity);

                _mapper.Map(transactionUpdateRequest, transactionDomain);
                var updatedTransaction = await _transactionRepository.UpdateTransactionAsync(transactionDomain, transactionEntity);

                var transactionResponse = _mapper.Map<TransactionResponse>(updatedTransaction);
                return ApiResponse<TransactionResponse>.SuccessResponse("Thay đổi thông tin giao dịch thành công!", 200, transactionResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<TransactionResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<TransactionSummaryResponse>> GetListTransactionAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist){
                    return ApiResponse<TransactionSummaryResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var transactions = await _transactionRepository.GetListTransactionAsync(idUser);
                var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);

                var expenseTransactions = transactionResponses // Lọc ra loại Chi
                    .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeExpense)
                    .ToList();

                if (!expenseTransactions.Any()){
                    return ApiResponse<TransactionSummaryResponse>.FailResponse(
                        "Người dùng chưa có giao dịch chi tiêu nào!", 
                        404);
                }

                var totalExpense = expenseTransactions.Sum(t => t.Amount); // Tổng chi tiêu

                // Gom nhóm theo category
                var chartData = expenseTransactions
                    .GroupBy(t => t.TransactionCategory)
                    .Select(group => new TransactionChartResponse
                    {
                        TransactionCategory = group.Key ?? "Khác",
                        ExpenseAmount = group.Sum(t => t.Amount),
                        ExpensePercent = totalExpense == 0 ? 0 : Math.Round(group.Sum(t => t.Amount) / totalExpense * 100, 2),
                    })
                    .OrderByDescending(c => c.ExpenseAmount)
                    .ToList();

                var summaryResponse = new TransactionSummaryResponse
                {
                    ExpenseList = transactionResponses,
                    ChartList = chartData
                };

                return ApiResponse<TransactionSummaryResponse>.SuccessResponse("Lấy danh sách giao dịch thành công!", 200, summaryResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<TransactionSummaryResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<TransactionResponse>>> GetListTransactionFullAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist){
                    return ApiResponse<List<TransactionResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var transactions = await _transactionRepository.GetListTransactionAsync(idUser);
                var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);

                if (!transactionResponses.Any())
                {
                    return ApiResponse<List<TransactionResponse>>.FailResponse(
                        "Người dùng chưa có giao dịch chi tiêu nào!",
                        404);
                }

                return ApiResponse<List<TransactionResponse>>.SuccessResponse("Lấy danh sách giao dịch thành công!", 200, transactionResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransactionResponse>>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<BriefTransactionResponse>> GetListBriefTransactionAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist){
                    return ApiResponse<BriefTransactionResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var transactions = await _transactionRepository.GetListBriefTransactionAsync(idUser);

                var totalTransaction = transactions.Count(); // Tổng số giao dịch trong 7 ngày qua

                var listBriefTransactionResponses = _mapper.Map<List<ListBriefTransactionResponse>>(transactions);

                if (!listBriefTransactionResponses.Any())
                {
                    return ApiResponse<BriefTransactionResponse>.FailResponse(
                        "Người dùng chưa có giao dịch chi tiêu nào trong 7 ngày qua!",
                        404);
                }

                var briefTransactionResponse = new BriefTransactionResponse
                {
                    totalTransactionInWeek = totalTransaction,
                    listBriefTransactionResponses = listBriefTransactionResponses
                };

                return ApiResponse<BriefTransactionResponse>.SuccessResponse("Lấy danh sách giao dịch thành công!", 200, briefTransactionResponse);
            }
            catch (Exception ex){
                return ApiResponse<BriefTransactionResponse>.FailResponse(ex.Message, 500);
            }
        }

        // Hàm so sánh giao dịch giữa 2 năm
        public async Task<ApiResponse<CompareTransactionByYearResponse>> CompareTransactionByYearAsync(CompareTransactionByYearRequest compareTransactionByYearRequest)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(compareTransactionByYearRequest.IdUser);
                if (!checkUserExist)
                {
                    return ApiResponse<CompareTransactionByYearResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var transactions = await _transactionRepository.GetTransactionsByUserAndYearsAsync(
                    compareTransactionByYearRequest.IdUser,
                    new[] { compareTransactionByYearRequest.Year1, compareTransactionByYearRequest.Year2 });

                if (!transactions.Any()){
                    return ApiResponse<CompareTransactionByYearResponse>.FailResponse("Không có giao dịch trong 2 năm này!", 404);
                }

                var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);

                var groupByYear = transactionResponses
                    .GroupBy(t => t.TransactionDate?.Year)
                    .ToDictionary(g => g.Key!.Value, g => g.ToList());

                // Lấy giao dịch của từng năm
                var listYear1 = groupByYear.GetValueOrDefault(compareTransactionByYearRequest.Year1, new List<TransactionResponse>());
                var listYear2 = groupByYear.GetValueOrDefault(compareTransactionByYearRequest.Year2, new List<TransactionResponse>());

                // Tính tổng thu và chi của mỗi năm
                var summaryYear1 = CalculateTransactionYearSummary(listYear1, compareTransactionByYearRequest.Year1);
                var summaryYear2 = CalculateTransactionYearSummary(listYear2, compareTransactionByYearRequest.Year2);

                var compareResponse = new CompareTransactionByYearResponse
                {
                    Year1Summary = summaryYear1,
                    Year2Summary = summaryYear2,
                };

                return ApiResponse<CompareTransactionByYearResponse>.SuccessResponse("So sánh giao dịch giữa hai năm thành công!", 200, compareResponse);
            }
            catch (Exception ex){
                return ApiResponse<CompareTransactionByYearResponse>.FailResponse(ex.Message, 500);
            }
        }

        // Hàm so sánh giao dịch giữa 2 tháng cùng hoặc khác năm
        public async Task<ApiResponse<CompareTransactionByMonthResponse>> CompareTransactionByMonthAsync(CompareTransactionByMonthRequest compareTransactionByMonthRequest)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(compareTransactionByMonthRequest.IdUser);
                if (!checkUserExist){
                    return ApiResponse<CompareTransactionByMonthResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var transactions = await _transactionRepository.GetTransactionsByUserAndMonthsAsync(
                    compareTransactionByMonthRequest.IdUser,
                    new[] 
                    { 
                        (compareTransactionByMonthRequest.FirstMonth, compareTransactionByMonthRequest.FirstYear),
                        (compareTransactionByMonthRequest.SecondMonth, compareTransactionByMonthRequest.SecondYear) 
                    });

                if (!transactions.Any()){
                    return ApiResponse<CompareTransactionByMonthResponse>.FailResponse("Không có giao dịch trong 2 tháng này!", 404);
                }

                var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);

                var groupByMonthYear = transactionResponses
                    .GroupBy(t => new { t.TransactionDate!.Value.Month, t.TransactionDate!.Value.Year })
                    .ToDictionary(g => (g.Key!.Month, g.Key!.Year), g => g.ToList());

                // Lấy giao dịch của từng tháng và năm
                groupByMonthYear.TryGetValue(
                    (compareTransactionByMonthRequest.FirstMonth, compareTransactionByMonthRequest.FirstYear),
                    out var month1List);

                groupByMonthYear.TryGetValue(
                    (compareTransactionByMonthRequest.SecondMonth, compareTransactionByMonthRequest.SecondYear),
                    out var month2List);

                month1List ??= new List<TransactionResponse>();
                month2List ??= new List<TransactionResponse>();

                // Tính tổng thu và chi của mỗi tháng và năm
                var summaryMonth1 = CalculateTransactionMonthSummary(month1List, 
                                                                     compareTransactionByMonthRequest.FirstMonth,
                                                                     compareTransactionByMonthRequest.FirstYear);
                var summaryMonth2 = CalculateTransactionMonthSummary(month2List,
                                                                     compareTransactionByMonthRequest.SecondMonth,
                                                                     compareTransactionByMonthRequest.SecondYear);

                var compareResponse = new CompareTransactionByMonthResponse
                {
                    Month1Summary = summaryMonth1,
                    Month2Summary = summaryMonth2,
                };

                return ApiResponse<CompareTransactionByMonthResponse>.SuccessResponse("So sánh giao dịch giữa hai tháng thành công!", 200, compareResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<CompareTransactionByMonthResponse>.FailResponse(ex.Message, 500);
            }
        }

        private YearlyTransactionSummary CalculateTransactionYearSummary(List<TransactionResponse> transactions, int year)
        {
            var incomeTransactions = transactions
                .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeCollect)
                .ToList();

            var expenseTransactions = transactions
                .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeExpense)
                .ToList();

            return new YearlyTransactionSummary
            {
                Year = year,
                TotalIncome = incomeTransactions.Sum(t => t.Amount),
                TransactionIncomeDetails = incomeTransactions.Select(t => new CompareTransactionDetailResponse
                {
                    IdTransaction = t.IdTransaction,
                    TransactionName = t.TransactionName,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionCategory = t.TransactionCategory,
                    TransactionDate = t.TransactionDate
                }).ToList(),

                TotalExpense = expenseTransactions.Sum(t => t.Amount),
                TransactionExpenseDetails = expenseTransactions.Select(t => new CompareTransactionDetailResponse
                {
                    IdTransaction = t.IdTransaction,
                    TransactionName = t.TransactionName,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionCategory = t.TransactionCategory,
                    TransactionDate = t.TransactionDate
                }).ToList(),

                SpreadIncomeAndExpenseByYear = incomeTransactions.Sum(t => t.Amount) - expenseTransactions.Sum(t => t.Amount)
            };
        }

        private MonthlyTransactionSummary CalculateTransactionMonthSummary(List<TransactionResponse> transactions, int month, int year)
        {
            var incomeTransactions = transactions
                .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeCollect)
                .ToList();

            var expenseTransactions = transactions
                .Where(t => t.TransactionType == ConstrantCollectAndExpense.TypeExpense)
                .ToList();

            return new MonthlyTransactionSummary
            {
                Year = year,
                Month = month,
                TotalIncome = incomeTransactions.Sum(t => t.Amount),
                TransactionIncomeDetails = incomeTransactions.Select(t => new CompareTransactionDetailResponse
                {
                    IdTransaction = t.IdTransaction,
                    TransactionName = t.TransactionName,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionCategory = t.TransactionCategory,
                    TransactionDate = t.TransactionDate
                }).ToList(),

                TotalExpense = expenseTransactions.Sum(t => t.Amount),
                TransactionExpenseDetails = expenseTransactions.Select(t => new CompareTransactionDetailResponse
                {
                    IdTransaction = t.IdTransaction,
                    TransactionName = t.TransactionName,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionCategory = t.TransactionCategory,
                    TransactionDate = t.TransactionDate
                }).ToList(),

                SpreadIncomeAndExpenseByMonth = incomeTransactions.Sum(t => t.Amount) - expenseTransactions.Sum(t => t.Amount)
            };
        }
    }
}
