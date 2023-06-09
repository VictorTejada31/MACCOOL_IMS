using Core.Application.Dtos.Account;
using Core.Application.ViewModels.Users;

namespace Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<RegisterResponse> CreateAdminFree(RegisterFreeViewModel viewModel);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel viewModel, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel viewModel);
        Task<AuthenticationResponse> SignInAsync(LoginViewModel viewModel);
        Task SignOut();
    }
}