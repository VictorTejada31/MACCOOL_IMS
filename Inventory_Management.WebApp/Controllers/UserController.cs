using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.WebApp.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;
       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            
            return View(new LoginViewModel());
        }


        //agregar sesion
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            AuthenticationResponse response = await _userService.SignInAsync(viewModel);

            if(!response.HasError && response != null)
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            viewModel.HasError = response.HasError;
            viewModel.Error = response.Error;
            return View(viewModel);
            
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOut();
            return RedirectToRoute(new { Controller = "User", Action = "Login" });
        }
        public IActionResult RegisterIndex()
        {
            return View();
        }

        public IActionResult FreeRegister()
        {
            return View(new RegisterFreeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> FreeRegister(RegisterFreeViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _userService.CreateAdminFree(viewModel);

            return RedirectToRoute(new { Controller = "User" , Action = "Login" });
        }



    }
}
