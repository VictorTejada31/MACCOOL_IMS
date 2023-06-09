
using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Users;

namespace Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> SignInAsync(LoginViewModel viewModel)
        {
            AuthenticationResponse response = await _accountService.SignInAsync(_mapper.Map<AuthenticationRequest>(viewModel));
            return response;
        }

        public async Task SignOut()
        {
            await _accountService.SignOut();
        }

        public async Task<RegisterResponse> CreateAdminFree(RegisterFreeViewModel viewModel)
        {
            RegisterRequest request = _mapper.Map<RegisterRequest>(viewModel);
            request.Plan = Plans.Free.ToString();

            RegisterResponse response = await _accountService.CreateAdmin(request);
            return response;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel viewModel, string origin)
        {
            ForgotPasswordResponse response = await _accountService.ForgotPasswordAsync(_mapper.Map<ForgotPasswordRequest>(viewModel), origin);
            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel viewModel)
        {
            ResetPasswordResponse response = await _accountService.ResetPasswordAsync(_mapper.Map<ResetPasswordRequest>(viewModel));
            return response;
        }
    }
}
