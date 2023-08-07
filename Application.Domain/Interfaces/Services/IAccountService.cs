
using Core.Application.Dtos.Account;

namespace Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> SignInAsync(AuthenticationRequest request);
        Task SignOut();
        Task<RegisterResponse> CreateAdmin(RegisterRequest request);
        Task<RegisterResponse> CreateCashier(RegisterCashierRequest request);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<List<CashierModelResponse>> GetAllCashier(string userId);
        Task<ChangeCashierStateResponse> ChangeCashierState(string userId);
        Task<bool> OnlineStatus(string userId, bool isOn);
        Task DeleteCashier(string userId);
        Task<UserByIdResponse> GetCashierById(string userId);
        Task<RegisterResponse> UpdateCashier(string userId, RegisterCashierRequest request);

    }
}
