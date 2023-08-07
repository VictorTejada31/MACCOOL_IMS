using AutoMapper;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Product;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;


namespace Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepositoy _category;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;
        private readonly IDashBoardService _boardService;
        public ProductService(IProductRepository repository,
            IMapper mapper,
            ICategoryRepositoy category,
            IHttpContextAccessor httpContextAccessor,
            IDashBoardService boardService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _category = category;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _boardService = boardService;
        }

        public async Task<ProductViewModel> GetByIdWithInclude(int id)
        {
            var products = await _repository.GetAllWithInclude(new List<string> { "DefaultProduct" });
            var product = products.Where(product => product.Id == id).FirstOrDefault();

            ProductViewModel model = new()
            {
                Id = product.Id,
                Amount = product.Amount,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                DefaultProudctId = product.DefaultProductId,
                CreatedBy = product.UserId
            };

            return model;
        }



        public async Task<List<ProductViewModel>> GetAllWithIncludes()
        {
            var products = await _repository.GetAllWithInclude(new List<string> { "DefaultProduct" });
            List<ProductViewModel> result = new();

            foreach (var product in products.Where(user => user.UserId == _userViewModel.Id))
            {
                ProductViewModel model = new()
                {
                    Id = product.Id,
                    Category = await CategoryName(product.DefaultProduct.CategoryId),
                    Amount = product.Amount,
                    BarCode = product.DefaultProduct.BarCode,
                    Description = product.DefaultProduct.Description,
                    Img = product.DefaultProduct.Img,
                    Name = product.DefaultProduct.Name,
                    PurchasePrice = product.PurchasePrice,
                    SalePrice = product.SalePrice,
                    DefaultProudctId = product.DefaultProductId,
                    CreatedBy = product.UserId
                };

                result.Add(model);
            }

            return result;
        }

        public async Task<ProductViewModel> GetProductByBarCode(string barcode)
        {
            var products = await _repository.GetAllWithInclude(new List<string> { "DefaultProduct" });
            var product = products.Where(pdt => pdt.DefaultProduct.BarCode == barcode && pdt.UserId == _userViewModel.CreatedBy).FirstOrDefault(); // filtrar por el usuario
            ProductViewModel vm = new();
           
            if(product != null)
            {
                vm.SalePrice = product.SalePrice;
                vm.Name = product.DefaultProduct.Name;
                vm.Id = product.Id;
                vm.Img = product.DefaultProduct.Img;
                vm.Category = await CategoryName(product.DefaultProduct.CategoryId);
                vm.Description = product.DefaultProduct.Description;
            }

            return vm;
        }
        private async Task<string> CategoryName(int id)
        {
            var category = await _category.GetByIdAsync(id);
            return category.Name;
        }

        public async Task UpdateInventory(List<string[]> array)
        {
            var a = array;
            foreach(var arr in array)
            {
                if (arr[0] == "")
                {
                    continue;
                }

                Product product = await _repository.GetByIdAsync(Int32.Parse(arr[0]));
                product.Amount--;
                await _repository.UpdateAsync(product,product.Id);
                await _boardService.UpdateDashBoard(_userViewModel.CreatedBy, product.SalePrice);


            }
        }
    }
}
            
           


          

            
            

        

       
        
