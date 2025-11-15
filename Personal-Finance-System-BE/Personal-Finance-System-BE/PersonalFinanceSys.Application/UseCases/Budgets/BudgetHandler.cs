using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Budgets
{
    public class BudgetHandler
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public BudgetHandler(IBudgetRepository budgetRepository, 
                             IUserRepository userRepository, 
                             IImageRepository imageRepository,
                             ITransactionRepository transactionRepository,
                             IMapper mapper)
        {
            _budgetRepository = budgetRepository;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateBudgetAsync(BudgetCreationRequest budgetCreationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(budgetCreationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người dùng!", 404);

                var budgetDomain = _mapper.Map<BudgetDomain>(budgetCreationRequest);
                var createdBudget = await _budgetRepository.AddBudgetAsync(budgetDomain);

                if (!string.IsNullOrEmpty(budgetCreationRequest.UrlImage))
                {
                    var image = new ImageDomain
                    {
                        Url = budgetCreationRequest.UrlImage,
                        IdRef = createdBudget.IdBudget,
                        RefType = "BUDGETS"
                    };
                    await _imageRepository.AddImageAsync(image);
                }

                return ApiResponse<string>.SuccessResponse("Tạo ngân sách thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteBudgetAsync(Guid idBudget)
        {
            try
            {
                await _budgetRepository.DeleteBudgetAsync(idBudget);
                var existingImageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idBudget, "BUDGETS");
                if (!string.IsNullOrEmpty(existingImageUrl))
                {
                    await _imageRepository.DeleteImageByIdRefAsync(idBudget, "BUDGETS");
                }

                return ApiResponse<string>.SuccessResponse("Xóa ngân sách thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<BudgetResponse>> UpdateBudgetAsync(Guid idBudget, BudgetUpdateRequest budgetUpdateRequest)
        {
            try
            {
                var budgetEntity = await _budgetRepository.GetExistBudget(idBudget);
                if (budgetEntity == null){
                    return ApiResponse<BudgetResponse>.FailResponse("Không tìm thấy ngân sách!", 404);
                }
                var budgetDomain = _mapper.Map<BudgetDomain>(budgetEntity);

                var oldBudgetName = budgetEntity.BudgetName; // Tên cũ
                var idUser = budgetEntity.IdUser;

                _mapper.Map(budgetUpdateRequest, budgetDomain);
                // Cập nhật ảnh nếu có
                if (!string.IsNullOrEmpty(budgetUpdateRequest.UrlImage))
                {
                    var existingImageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idBudget, "BUDGETS");
                    if (!string.IsNullOrEmpty(existingImageUrl))
                    {
                        await _imageRepository.DeleteImageByIdRefAsync(idBudget, "BUDGETS");
                    }

                    var image = new ImageDomain
                    {
                        Url = budgetUpdateRequest.UrlImage,
                        IdRef = idBudget,
                        RefType = "BUDGETS"
                    };

                    await _imageRepository.AddImageAsync(image);
                }

                // Cập nhật tên ngân sách trong các giao dịch liên quan nếu tên ngân sách thay đổi
                if (!string.Equals(oldBudgetName, budgetDomain.BudgetName, StringComparison.OrdinalIgnoreCase))
                {
                    await _transactionRepository.UpdateTransactionCategoryByBudgetNameAsync(
                        idUser,
                        oldBudgetName,
                        budgetDomain.BudgetName
                    );
                }

                var updatedBudget = await _budgetRepository.UpdateBudgetAsync(budgetDomain, budgetEntity);

                var budgetResponse = _mapper.Map<BudgetResponse>(updatedBudget);
                return ApiResponse<BudgetResponse>.SuccessResponse("Thay đổi thông tin ngân sách thành công!", 200, budgetResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<BudgetResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<BudgetResponse>>> GetListBudgetAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist)
                {
                    return ApiResponse<List<BudgetResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }
                var budgetDomains = await _budgetRepository.GetListBudgetAsync(idUser);
                if (!budgetDomains.Any())
                {
                    return ApiResponse<List<BudgetResponse>>.SuccessResponse(
                        "Người dùng chưa có ngân sách nào!",
                        200,
                        new List<BudgetResponse>());
                }

                var expenseTransactions = await _transactionRepository
                    .GetExpenseTransactionsByUserAsync(idUser);

                // Gom nhóm theo TransactionCategory
                var transactionSums = expenseTransactions
                    .GroupBy(t => t.TransactionCategory)
                    .ToDictionary(
                        g => g.Key ?? string.Empty,
                        g => g.Sum(t => t.Amount)
                    );

                // Lấy danh sách id ngân sách để truy xuất ảnh
                var idRefs = budgetDomains.Select(b => b.IdBudget).ToList();
                var imageDict = await _imageRepository.GetImagesByListRefAsync(idRefs, "BUDGETS");

                var budgetResponses = budgetDomains.Select(budget =>
                {

                    var currentBudget = transactionSums.TryGetValue(budget.BudgetName, out var sum) ? sum : 0;

                    var progress = budget.BudgetGoal > 0
                        ? Math.Round((double)(currentBudget / budget.BudgetGoal) * 100, 2)
                        : 0;

                    return new BudgetResponse
                    {
                        IdBudget = budget.IdBudget,
                        BudgetName = budget.BudgetName,
                        CurrentBudget = currentBudget,
                        BudgetProgress = progress,
                        BudgetGoal = budget.BudgetGoal,
                        UrlImage = imageDict.ContainsKey(budget.IdBudget) ? imageDict[budget.IdBudget] : null
                    };
                }).ToList(); return ApiResponse<List<BudgetResponse>>.SuccessResponse("Lấy danh sách giao dịch thành công!", 200, budgetResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BudgetResponse>>.FailResponse(ex.Message, 500);
            }
        }
    }
}
