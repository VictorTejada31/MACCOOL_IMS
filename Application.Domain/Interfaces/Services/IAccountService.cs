

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
    }
}
