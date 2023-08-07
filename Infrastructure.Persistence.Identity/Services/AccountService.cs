using Core.Application.Dtos.Account;
using Core.Application.Dtos.Email;
using Core.Application.Enums;
using Core.Application.Helpers;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.DashBoard;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly IEmailService _emailService;
        private readonly IDashBoardService _boardService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;

        public AccountService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> singInManager,
            IEmailService emailService, 
            IDashBoardService dashBoardService,
            IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _emailService = emailService;
            _boardService = dashBoardService;
            _httpContextAccessor = httpContext;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<AuthenticationResponse> SignInAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new() { HasError = false };
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"{request.Email} no se encuentra registrado aún.";
                return response;
            }


            var result = await _singInManager.PasswordSignInAsync(user, request.Password,true,false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Contraseña incorrecta.";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"{request.Email} no se encuentra activado, hable con su administrador.";
                return response;
            }

            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.Id = user.Id;
            response.EmailConfirmed = user.EmailConfirmed;
            response.CreatedBy = user.CretedBy;
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = roles.ToList();

            return response;
        }
        public async Task SignOut()
        {
           await _singInManager.SignOutAsync();
        }
        public async Task<RegisterResponse> CreateAdmin(RegisterRequest request)
        {
            RegisterResponse response = new() { HasError = true };
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
            
            if(user != null)
            {
                response.HasError = true;
                response.Error = $"{request.Email} ya se encuentra registrado, por favor utilice un correo diferente.";
                return response;
            }

            ApplicationUser newUser = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                isOnline = false,
                LastConnection = DateTime.Now,
                EmailConfirmed = true,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                Plan = request.Plan,
                CretedBy = "Itself"
            };

            var result = await _userManager.CreateAsync(newUser,request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Error while creating User";
                return response;
            }
            
            await _userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
            await _emailService.SendAsync(new EmailRequest
            {
                From = "Mccool Inventory",
                To = newUser.Email,
                Subject = "Mccool Inventory - Correo de Bienvenida",
                Body = $"<p> Gracias por registrarte a Mccool Inventory, tu plan actaul es {newUser.Plan}</p>"
                
            });

            SaveDashBoardViewModel saveDashBoardViewModel = new SaveDashBoardViewModel() { 
                    Today = 0,
                    LastMonth = 0,
                    ThisMonth = 0,
                    Yesterday = 0,
                    ThisMonthDate = DateTime.Now,
                    TodayDate = DateTime.Now,
                    UserId = newUser.Id,
            };

            await _boardService.AddAsync(saveDashBoardViewModel);

            return response; 
        }

        public async Task<RegisterResponse> CreateCashier(RegisterCashierRequest request)
        {
            RegisterResponse response = new() { HasError = true };

           // if (await CheckingIfCanCreate(_userViewModel.Id))
            //{
              //  response.HasError = true;
               // response.Error = "Ya estas al limite de cajeros para agregar otro cajero adquiera el plan premium.";
               // return response;
           // }

            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                response.HasError = true;
                response.Error = $"{request.Email} ya se encuentra registrado, por favor utilice un correo diferente.";
                return response;
            }

            ApplicationUser newUser = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                isOnline = false,
                LastConnection = DateTime.Now,
                EmailConfirmed = true,
                UserName = request.Email,
                PhoneNumber = request.Tel,
                CretedBy = _userViewModel.Id,
                Plan = Roles.Cashier.ToString(),
                

            };


            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Error while creating User";
                return response;
            }

            await _userManager.AddToRoleAsync(newUser, Roles.Cashier.ToString());

            return response;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new() { HasError = false};
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"{request.Email} no se encuentra registrado.";
                return response;
            }
            string uri = await SendForgotPassEmailUri(user,origin);
            EmailRequest emailRequest = new()
            {
                From = "Mccool Inventory System",
                To = request.Email,
                Subject = "Mccool Inventory System -Recuperar Contraseña",
                Body = $"<p>Click al siguiente enlace para resetear tu contraseña </p> <br> {uri}"
            };

            await _emailService.SendAsync(emailRequest);

            return response;

        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new() { HasError = false };
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"{request.Email} no se encuentra registrado en el sistema.";
                return response;
            }


            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token)); 
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Error mientras actualizaba contraseña, por favor intentolo de nuevo.";
                return response;
            }

            return response;

        }

        public async Task<List<CashierModelResponse>> GetAllCashier(string userId)
        {
            var clients =  await _userManager.Users.Where(c => c.CretedBy == userId).ToListAsync();
            List<CashierModelResponse> response = new();

            foreach (var client in clients)
            {
                CashierModelResponse cashierModelResponse = new CashierModelResponse()
                {
                    isActive = client.EmailConfirmed,
                    isOnline = client.isOnline,
                    Name = $"{client.FirstName} {client.LastName}",
                    CashierId = client.Id
                };

                response.Add(cashierModelResponse);
            }

            return response;
            
        }


        public async Task<UserByIdResponse> GetCashierById(string userId)
        {
            var client = await _userManager.FindByIdAsync(userId);

            UserByIdResponse response = new UserByIdResponse()
            {
                Id = client.Id,
                Email = client.Email,
                UserName = client.UserName,
                Tel = client.PhoneNumber,
                FirstName = client.FirstName,
                LastName = client.LastName
            };

            return response;

        }

        public async Task<RegisterResponse> UpdateCashier(string userId, RegisterCashierRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            RegisterResponse response = new RegisterResponse();
            var findbyEmail = await _userManager.FindByEmailAsync(request.Email);

            if (findbyEmail != null && user.Email != request.Email)
            {
                response.HasError = true;
                response.Error = "Email registrado, utilize otra direccion de correo.";
                return response;
            }

            var findbyUserName = await _userManager.FindByNameAsync(request.UserName);


            if (findbyUserName != null && user.UserName != request.UserName)
            {
                response.HasError = true;
                response.Error = "Nombre de usuario registrado, utilize otro nombre de usuario.";
                return response;
            }


            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.PhoneNumber = request.Tel;

            await _userManager.UpdateAsync(user);
            
            if(request.Password != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user,token,request.Password);

                if (!result.Succeeded)
                {
                    response.HasError = true;
                    response.Error = "Error al cambiar contraseña";
                    return response;
                }
            }

            return response;

        }


        public async Task<bool> OnlineStatus(string userId, bool isOn)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (isOn)
            {
                user.isOnline = true;
            }
            else
            {
                user.isOnline = false;
            }

            await _userManager.UpdateAsync(user);
            return true;



        }

        public async Task<ChangeCashierStateResponse> ChangeCashierState(string userId)
        {
            ChangeCashierStateResponse response = new() { HasError = false};
            var user = await _userManager.FindByIdAsync(userId);

            if (user.EmailConfirmed)
            {
                user.EmailConfirmed = false;
                var _result = await _userManager.UpdateAsync(user);
                if (!_result.Succeeded)
                {
                    response.HasError = true;
                    return response;
                }
                
                response.Message = "Cuenta desactivada correctamente.";
            }
            else
            {

                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                response.Message = "Cuenta activada correctamente.";
            }

            return response; 


        }

        public async Task DeleteCashier(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
        }

        private async Task<bool> CheckingIfCanCreate(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var cashiers = await _userManager.Users.Select(x => x.CretedBy == userId).ToListAsync();

            if (user.Plan == Plans.Free.ToString() && cashiers.Count <= 1)
            {
                return true;
            }
            else if (user.Plan == Plans.Standar.ToString() && cashiers.Count <= 3)
            {
                return true;
            }
            else if (user.Plan == Plans.Premium.ToString() && cashiers.Count <= 5)
            {
                return true;
            }

            return false;
        }

        private async Task<string> SendForgotPassEmailUri(ApplicationUser user, string origin)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            string route = "/User/ResetPass";
            Uri uri = new Uri(String.Concat(origin,route));
            string verificationUri = QueryHelpers.AddQueryString(uri.ToString(), "token", token);

            return verificationUri;
        }


    }
}
