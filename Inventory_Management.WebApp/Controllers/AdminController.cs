using Core.Application.Helpers;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Client;
using Core.Application.ViewModels.DefaultProducts;
using Core.Application.ViewModels.Product;
using Core.Application.ViewModels.RegisterCash;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IDefaultProductService _defaultProductService;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;
        private readonly IDashBoardService _boardService;
        private readonly IUserService _userService;

        public AdminController(IClientService clientService, 
            IDefaultProductService defaultProductService, 
            IProductService productService,
            IHttpContextAccessor httpContextAccessor,
            IDashBoardService dashBoardService,
            IUserService userService)
        {
            _clientService = clientService;
            _defaultProductService = defaultProductService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _boardService = dashBoardService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DashBoard()
        {
            return View(await _boardService.GetAllAsync());
        }

        public async Task<IActionResult> DefaultProducts()
        {
            ViewBag.defaultPrdoucts = await _defaultProductService.GetAllWithInclude();
            return View();
        }

        public async Task<IActionResult> Client()
        {
            ViewBag.Clients = await _clientService.GetAllAsync();
            return View(new SaveClientViewModel() { Id = 0});
        }

        [HttpPost]
        public async Task<IActionResult> Client(SaveClientViewModel viewModel)
        {
            if (viewModel.FullName == null)
            {
               ViewBag.Clients = await _clientService.GetAllAsync();
                return View(viewModel);
           }

            viewModel.CreatedBy = _userViewModel.Id;
            await _clientService.AddAsync(viewModel);
            return RedirectToRoute(new { Controller = "Admin", Action = "Client" });
        }

        public async Task<IActionResult> DeleteClient(int id)
        {
            await _clientService.DeleteAsync(id);
            return RedirectToRoute(new { Controller = "Admin", Action = "Client" });
        }

        public async Task<IActionResult> UpdateClient(int id)
        {
            var client = await _clientService.GetById(id);
            SaveClientViewModel viewModel = new()
            {
                FullName = client.FullName,
                IdCard = client.IdCard,
                Owed = client.Owed,
                Tel = client.Tel,
                Id = client.Id
            };

            ViewBag.Clients = await _clientService.GetAllAsync();
            return View("Client", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClient(SaveClientViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.CreatedBy = _userViewModel.Id;
            await _clientService.UpdateAsync(viewModel, (int)viewModel.Id);
            return RedirectToRoute(new { Controller = "Admin", Action = "Client" });

        }


        public async Task<IActionResult> Product()
        {
            return View(await _productService.GetAllWithIncludes());
        }


        [HttpPost]
        public async Task<IActionResult> Product(SaveProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.UserId = _userViewModel.Id;
            await _productService.AddAsync(viewModel);
            return RedirectToRoute(new { Controller = "Admin", Action = "DefaultProducts" });
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToRoute(new { Controller = "Admin", Action = "Product" });

        }

        public async Task<IActionResult> EditProduct(int id)
        {
            ProductViewModel productView = await _productService.GetByIdWithInclude(id);
            SaveProductViewModel viewModel = new()
            {
                PurchasePrice = (int)productView.PurchasePrice,
                SalePrice = (int)productView.SalePrice,
                Amount = productView.Amount,
                Id = id,
                DefaultProductId = productView.DefaultProudctId,
                UserId = productView.CreatedBy
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(SaveProductViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }


            await _productService.UpdateAsync(viewModel, viewModel.Id);
            return RedirectToRoute(new { Controller = "Admin", Action = "Product" });

        }


        public async Task<IActionResult> CashRegister()
        {
            ViewBag.registerCash = await _userService.GetCashiersAsync(_userViewModel.Id);
            return View();
        }

        public async Task<IActionResult> EditCashRegister(string id)
        {
            var cashier = await _userService.GetCashierById(id);
            EditCashierViewModel viewModel = new()
            {
                Email = cashier.Email,
                FirstName = cashier.FirstName,
                LastName = cashier.LastName,
                Tel = cashier.Tel,
                UserName = cashier.UserName,
            };

            ViewBag.Error = false;
            return View(cashier);
        }

        [HttpPost]
        public async Task<IActionResult> EditCashRegister(EditCashierViewModel viewModel)
        {
            if(!ModelState.IsValid) {

                ViewBag.Error = false;
                return View(viewModel);

            }

            var result = await _userService.UpdateCashier(viewModel.Id,viewModel);

            if (result.HasError)
            {
                ViewBag.Error = result.HasError;
                ViewBag.Message = result.Error;
                return View(viewModel);
            }

            return RedirectToRoute(new {Controller = "Admin", Action = "CashRegister"});
        }


        [HttpPost]
        public async Task<IActionResult> ProductResult(string searchProduct)
        {
            if (searchProduct == null)
            {
              return  RedirectToRoute(new { Controller = "Admin", Action = "Product"});
            }
            var products = await _productService.GetAllWithIncludes();
            var _products = products.Where(p => p.Name.StartsWith(searchProduct)).ToList();
            return View(_products);
        }

        [HttpPost]
        public async Task<IActionResult> DefaultProductResult(string searchProduct)
        {
            if (searchProduct == null)
            {
                return RedirectToRoute(new { Controller = "Admin", Action = "Product" });
            }


            var result = await _defaultProductService.GetAllAsync();
            ViewBag.defaultPrdoucts = result.Where(p => p.Name.StartsWith(searchProduct)).ToList();
            return View();

        }
    }
}
