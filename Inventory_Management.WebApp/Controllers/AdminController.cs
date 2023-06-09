using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.WebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
