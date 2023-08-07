using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using GoogleAuthentication.Services;
using Newtonsoft.Json;

namespace Inventory_Management.WebApp.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;
       
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            var clientId = "97044671162-rmebn860hohtr133vifs3bn1v1smu8h4.apps.googleusercontent.com";
            var url = "https://localhost:7002/User/GoogleLoginCallBack";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.UrlGoogleLogin = response;
            return View(new LoginViewModel());
        }
        
        public async Task<IActionResult> GoogleLoginCallBack(string code)
        {
            var clientId = "97044671162-rmebn860hohtr133vifs3bn1v1smu8h4.apps.googleusercontent.com";
            var url = "https://localhost:7002/User/GoogleLoginCallBack";
            var clientSecret = "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";
            var token = GoogleAuth.GetAuthAccessToken(code,clientId,clientSecret,url);
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.ToString());
            GoogleSignInViewModel googleSignInView = JsonConvert.DeserializeObject<GoogleSignInViewModel>(userProfile);
            return NoContent();
        }

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
                HttpContext.Session.Set<AuthenticationResponse>("user",response);
                return RedirectToRoute(new { Controller = "Home", Action = "Index" });
            }

            viewModel.HasError = response.HasError;
            viewModel.Error = response.Error;
            return View(viewModel);
            
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOut();
            HttpContext.Session.Remove("user");
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

        public IActionResult PremiumRegister()
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

        [HttpPost]
        public async Task<IActionResult> CreateCashier(RegisterCashierViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _userService.CreateCashier(viewModel);
            return RedirectToRoute(new { Controller = "Admin", Action = "CashRegister" });
        }

        public async Task<IActionResult> ChangeCashierState(string id)
        {
            await _userService.ChangeCashierState(id);
            return RedirectToRoute(new {Controller = "Admin", Action = "CashRegister" });
        }

        public async Task<IActionResult> DeleteCashier(string id)
        {
            await _userService.DeleteCashier(id);
            return RedirectToRoute(new { Controller = "Admin", Action = "CashRegister" });

        }


        public async Task<IActionResult> ForgotPassword()
        {
            return View(new ResetPasswordViewModel() { HasError = false});
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

           // await _userService.ForgotPasswordAsync(viewModel);
            return View();
        }
    }
}
