using Core.Application.Helpers;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Product;
using Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;


namespace Core.Application.Hubs
{
    public class CashierHub : Hub
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        public CashierHub(IProductService productService,
            IHttpContextAccessor httpContextAccessor,
            IClientService client,
            IUserService userService)
        {

            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _clientService = client;
            _userService = userService;
        }


        public override Task OnConnectedAsync()
        {
             _userService.OnlineStatus(_userViewModel.Id, true).GetAwaiter();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userService.OnlineStatus(_userViewModel.Id, false).GetAwaiter();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task<List<string>> AddNewProductToBill(string barcode)
        {
            ProductViewModel result = await _productService.GetProductByBarCode(barcode);
            if (result.Name != null)
            {
                
                return new List<string> { result.Id.ToString(),
                                        result.Name,
                                        result.SalePrice.ToString(),
                                        result.Description,
                                        result.Category,
                                        result.Img
                };
            }

            return null;

        }

        public async Task ProcessBill(List<string[]> array)
        {
            await _productService.UpdateInventory(array);
        }

        public async Task ProcessBillClients(string clientId, int total)
        {
            await _clientService.ProcessClienttBill(Int32.Parse(clientId), total);
        }

        public async Task SearchProduct(string word){
            var products = await _productService.GetAllWithIncludes();
            List<Dictionary<string, string>> result = new();
            foreach (var item in products.Where(p => p.Name.StartsWith(word)))
            {
               Dictionary<string,string> dictionary = new Dictionary<string,string>();
               dictionary.Add("Name", item.Name);
            }

            await Clients.All.SendAsync("Update",result);
        } 
    }
}
