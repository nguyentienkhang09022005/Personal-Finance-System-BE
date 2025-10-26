using AutoMapper;
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

        public async Task<ApiResponse<List<TransactionResponse>>> GetListTransactionAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist)
                {
                    return ApiResponse<List<TransactionResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }
                var transactions = await _transactionRepository.GetListTransactionAsync(idUser);
                var transactionResponses = _mapper.Map<List<TransactionResponse>>(transactions);
                return ApiResponse<List<TransactionResponse>>.SuccessResponse("Lấy danh sách giao dịch thành công!", 200, transactionResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransactionResponse>>.FailResponse(ex.Message, 500);
            }
        }
    }
}
