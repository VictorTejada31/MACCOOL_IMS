using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Client;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IDefaultProductService _defaultProductService;
        private readonly IProductService _productService;
        public AdminController(IClientService clientService, IDefaultProductService defaultProductService, IProductService productService)
        {
            _clientService = clientService;
            _defaultProductService = defaultProductService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DefaultProducts()
        {
            return View(await _defaultProductService.GetAllAsync());
        }

        public async Task<IActionResult> Client()
        {
            ViewBag.Clients = await _clientService.GetAllAsync();
            return View(new SaveClientViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Client(SaveClientViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

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

            await _clientService.UpdateAsync(viewModel, (int)viewModel.Id);
            return RedirectToRoute(new { Controller = "Admin", Action = "Client" });

        }
    }
}
