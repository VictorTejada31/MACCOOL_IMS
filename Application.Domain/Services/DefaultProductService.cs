using AutoMapper;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.DefaultProducts;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Services
{
    public class DefaultProductService : GenericService<SaveDefaultProductViewModel, DefaultProductViewModel, DefaultProduct>, IDefaultProductService
    {
        private readonly IDefaultProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;
        public DefaultProductService(IDefaultProductRepository repository, IMapper mapper, IProductService productService, IHttpContextAccessor httpContext) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _productService = productService;
            _httpContextAccessor = httpContext;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<List<DefaultProductViewModel>> GetAllWithInclude()
        {
            var products = await _repository.GetAllWithInclude(new List<string> { "Category", "Products" });
            return products.Select(p => new DefaultProductViewModel
            {
                Name = p.Name,
                BarCode = p.BarCode,
                Category = p.Category.Name,
                CategoryId = p.CategoryId,
                Description = p.Description,
                Id = p.Id,
                Img = p.Img,
                isAdded = isAdded(p.Id).GetAwaiter().GetResult(),


            }).ToList();
        }

        public async Task<bool> isAdded(int defaultProductId)
        {
            var product = await _productService.GetAllWithIncludes();
            var result = product.Where(p => p.DefaultProudctId == defaultProductId || p.CreatedBy == _userViewModel.CreatedBy).FirstOrDefault();
            if(result == null)
            {
                return false;
            }

            return true;
        }
    }
}
