using AutoMapper;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.DashBoard;
using Core.Application.ViewModels.Product;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Services
{
    public class DashBoardService : GenericService<SaveDashBoardViewModel, DashBoardViewModel, DashBoard>, IDashBoardService
    {
        private readonly IDashBoardRepository _dashBoardRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;
        public DashBoardService(IDashBoardRepository dashBoard,
            IMapper mapper,
            IProductRepository productRepository,
            IHttpContextAccessor httpContextAccessor
            ) : base(dashBoard, mapper)
        {
            _dashBoardRepository = dashBoard;
            _mapper = mapper;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task UpdateDashBoard(string userId, double price)
        {
            var dashBoards = await _dashBoardRepository.GetAllAsync();
            DashBoard board = dashBoards.Where(user => user.UserId == userId).FirstOrDefault();

            if (DateTime.Now.Day == board.TodayDate.Day && DateTime.Now.Month == board.TodayDate.Month)
            {
                board.Today += price;
            }
            else
            {
                board.Yesterday = board.Today;
                board.Today = 0;
                board.TodayDate = DateTime.Now;
            }

            if (DateTime.Now.Month == board.ThisMonthDate.Month)
            {
                board.ThisMonth += price;
            }
            else
            {
                board.LastMonth = board.ThisMonth;
                board.ThisMonth = 0;
                board.ThisMonthDate = DateTime.Now;
            }

            await _dashBoardRepository.UpdateAsync(board, board.Id);

        }

        public async Task<DashBoardViewModel> GetAllAsync()
        {
            var dashBoards = await _dashBoardRepository.GetAllAsync();
            var products = await _productRepository.GetAllWithInclude(new List<string> { "DefaultProduct" });
            List<ProductViewModel> productViews = new();
            List<ProductViewModel> topProducts = new();


            foreach (var product in products.Where(p => p.UserId == _userViewModel.Id).Take(5).ToList())
            {
                ProductViewModel productView = new ProductViewModel()
                {
                    Name = product.DefaultProduct.Name,
                    Amount = product.Amount
                };

                topProducts.Add(productView);

            }

            foreach (var product in products.Where(p => p.UserId == _userViewModel.Id && p.Amount < 20))
            {
                ProductViewModel  productView = new ProductViewModel()
                {
                    Amount = product.Amount,
                    Name = product.DefaultProduct.Name,
                    BarCode = product.DefaultProduct.BarCode,
                    Img = product.DefaultProduct.Img
                };

                productViews.Add(productView);

            }

            DashBoard board = dashBoards.Where(user => user.UserId == _userViewModel.Id).FirstOrDefault();
            DashBoardViewModel dashboardViewModel = new DashBoardViewModel()
            {
                ThisMonth = board.ThisMonth,
                Today = board.Today,
                Products = products.Where(p => p.UserId == _userViewModel.Id).Count(),
                TopProducts = topProducts, 
                AlertProduct = productViews
            };

            if(topProducts.Count < 5)
            {
                for(int i = 0; i < 5; i++)
                {
                    ProductViewModel productView = new ProductViewModel();
                    topProducts.Add(productView);
                }
            }

            return dashboardViewModel;
        }
    }
}
