using Core.Application.Dtos.Account;
using Core.Application.ViewModels.Admin;
using Core.Application.ViewModels.Users;

namespace Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<RegisterResponse> CreateAdminFree(RegisterFreeViewModel viewModel);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel viewModel, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel viewModel);
        Task<AuthenticationResponse> SignInAsync(LoginViewModel viewModel);
        Task<RegisterResponse> CreateCashier(RegisterCashierViewModel viewModel);
        Task SignOut();
        Task<List<CashierViewModel>> GetCashiersAsync(string UserId);
        Task<ChangeCashierStateResponse> ChangeCashierState(string userId);
        Task OnlineStatus(string userId, bool isOnline);
        Task DeleteCashier(string userId);
        Task<EditCashierViewModel> GetCashierById(string userId);
        Task<RegisterResponse> UpdateCashier(string userId, EditCashierViewModel editCashierView);
    }
}