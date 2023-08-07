using Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Core.Application.Helpers;
using Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Inventory_Management.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }
        public IActionResult Index()
        {
            if (_userViewModel.Roles.Any(r => r == Roles.Admin.ToString()))
            {
                return RedirectToRoute(new {Controller = "Admin", Action = "DashBoard" });
            }
            else if (_userViewModel.Roles.Any(r => r == Roles.Developer.ToString()))
            {
                return RedirectToRoute(new { Controller = "Developer", Action = "Product" });
            }
            else
            {
                return RedirectToRoute(new { Controller = "Cashier", Action = "Index" });
                
            }

          
        }


    }
}