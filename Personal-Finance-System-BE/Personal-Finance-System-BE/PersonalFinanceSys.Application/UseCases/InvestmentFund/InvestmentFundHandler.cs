using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund
{
    public class InvestmentFundHandler
    {
        private readonly IInvestmentFundRepository _investmentFundRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public InvestmentFundHandler(IInvestmentFundRepository investmentFundRepository, 
                                     IMapper mapper,
                                     IUserRepository userRepository)
        {
            _investmentFundRepository = investmentFundRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        // Hàm lấy danh sách quỹ cá nhân qua idUser
        public async Task<ApiResponse<List<InvestmentFundResponse>>> GetListInvestmentFundHandleAsync(Guid idUser)
        {
            bool userExists = await _userRepository.ExistUserAsync(idUser);
            if (!userExists)
                return ApiResponse<List<InvestmentFundResponse>>.FailResponse("Không tìm thấy người dùng!", 404);

            var funds = await _investmentFundRepository.GetListInvesmentFundDomains(idUser);

            var fundResposne = _mapper.Map<List<InvestmentFundResponse>>(funds);
            return ApiResponse<List<InvestmentFundResponse>>.SuccessResponse("Lấy danh sách quỹ của người dùng thành công!", 200, fundResposne);
        }

        // Hàm lấy thông tin chi tiết quỹ cá nhân qua ID (còn thiếu chi tiết của quỹ)
        public async Task<ApiResponse<InvestmentFundResponse>> GetInfInvestmentFundHandleAsync(Guid idFund)
        {
            try
            {
                var fund = await _investmentFundRepository.GetInfInvestmentFundAsync(idFund);

                var fundResposne = _mapper.Map<InvestmentFundResponse>(fund);
                return ApiResponse<InvestmentFundResponse>.SuccessResponse("Lấy thông tin chi tiết quỹ của người dùng thành công!", 200, fundResposne);
            } catch (NotFoundException ex)
            {
                return ApiResponse<InvestmentFundResponse>.FailResponse(ex.Message, 404);
            }
        }

        // Hàm tạo quỹ cá nhân
        public async Task<ApiResponse<InvestmentFundResponse>> CreateInvestmentHandleAsync(InvestmentFundCreationRequest investmentFundCreationRequest)
        {
            bool userExists = await _userRepository.ExistUserAsync(investmentFundCreationRequest.IdUser);
            if (!userExists)
                return ApiResponse<InvestmentFundResponse>.FailResponse("Không tìm thấy người dùng!", 404);

            var investmentFundDomain = _mapper.Map<InvestmentFundDomain>(investmentFundCreationRequest);
            var investmentFundCreated = await _investmentFundRepository.AddInvestmentAsync(investmentFundDomain);

            var investmentFundResponse = _mapper.Map<InvestmentFundResponse>(investmentFundCreated);
            return ApiResponse<InvestmentFundResponse>.SuccessResponse("Tạo quỹ thành công!", 200, investmentFundResponse);
        }

        // Hàm thay đổi thông tin của quỹ
        public async Task<ApiResponse<InvestmentFundResponse>> UpdateInvestmentFundAsync(Guid idFund ,InvestmentFundUpdateRequest investmentFundUpdateRequest)
        {
            try
            {
                var investmentFundEntity = await _investmentFundRepository.ExistInvestmentFund(idFund);
                var investmentDomain = _mapper.Map<InvestmentFundDomain>(investmentFundEntity);

                _mapper.Map(investmentFundUpdateRequest, investmentDomain);
                var investmentFundUpdated = await _investmentFundRepository.UpdateInvestmentFundAsync(investmentDomain, investmentFundEntity);

                var investmentFundResponse = _mapper.Map<InvestmentFundResponse>(investmentFundUpdated);
                return ApiResponse<InvestmentFundResponse>.SuccessResponse("Cập nhật thông tin quỹ thành công", 200, investmentFundResponse);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<InvestmentFundResponse>.FailResponse(ex.Message, 404);
            }
        }

        // Hàm xóa quỹ
        public async Task<ApiResponse<string>> DeleteInvestmentHandleAsync(Guid idFund)
        {
            try
            {
                await _investmentFundRepository.DeleteInvestmentFundAsync(idFund);
                return ApiResponse<string>.SuccessResponse("Xóa quỹ thành công!", 200, string.Empty);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 404);
            }
        }
    }
}
