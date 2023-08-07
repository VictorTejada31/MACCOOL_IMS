using Core.Application.ViewModels.Product;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {
        Task<List<ProductViewModel>> GetAllWithIncludes();
        Task<ProductViewModel> GetProductByBarCode(string barcode);
        Task UpdateInventory(List<string[]> array);

        Task<ProductViewModel> GetByIdWithInclude(int id);
    }
}
