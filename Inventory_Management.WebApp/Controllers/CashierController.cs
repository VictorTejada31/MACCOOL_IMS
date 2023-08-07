using Core.Application.Enums;
using Core.Application.Helpers;
using Core.Application.Hubs;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Product;
using Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace Inventory_Management.WebApp.Controllers
{
    [Authorize(Roles = "Cashier")]
    
    public class CashierController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHubContext<CashierHub> _hubContext;
        private readonly IClientService _clientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;
        public CashierController(IProductService productService, 
            IHubContext<CashierHub> hubContext,
            IClientService clientService,
            IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _hubContext = hubContext;
            _clientService = clientService;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Clients = await _clientService.GetClientByUserId(_userViewModel.CreatedBy);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(string barcode)
        {
            ProductViewModel result = await _productService.GetProductByBarCode(barcode);
            await _hubContext.Clients.All.SendAsync("AddNewProduct", result.Id,
                result.Name, 
                result.SalePrice, 
                result.Description, 
                result.Category, 
                result.Img);

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> AddProductScanner(string barcode)
        {
            ProductViewModel result = await _productService.GetProductByBarCode(barcode);
            await _hubContext.Clients.All.SendAsync("AddNewProduct", result.Id,
                result.Name,
                result.SalePrice,
                result.Description,
                result.Category,
                result.Img);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductOther(string name, int price)
        {
            
            await _hubContext.Clients.All.SendAsync("AddNewProductOther", name, price);
            return NoContent();
        }


    }
}
