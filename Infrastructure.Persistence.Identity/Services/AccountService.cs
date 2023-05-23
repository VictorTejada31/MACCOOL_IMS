using Core.Application.Dtos.Account;
using Core.Application.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Identity.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
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
                IdCard = request.IdCard,
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
                IdCard = request.IdCard,
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
    }
}
