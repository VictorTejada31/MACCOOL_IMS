using Core.Application.Dtos.Account;
using Core.Application.Dtos.Email;
using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Infrastructure.Identity.Entities;
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

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager,IEmailService emailService)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _emailService = emailService;
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

            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.Id = user.Id;
            response.EmailConfirmed = user.EmailConfirmed;
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

            return response; 
        }

        public async Task<RegisterResponse> CreateCashier(RegisterCashierRequest request)
        {
            RegisterResponse response = new() { HasError = true };

            if (!await CheckingIfCanCreate(request.CretedBy))
            {
                response.HasError = true;
                response.Error = "Ya estas al limite de cajeros para agregar otro cajero adquiera el plan premium.";
                return response;
            }

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
                EmailConfirmed = false,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                CretedBy = request.CretedBy,

            };


            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Error while creating User";
                return response;
            }

            await _userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());

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
