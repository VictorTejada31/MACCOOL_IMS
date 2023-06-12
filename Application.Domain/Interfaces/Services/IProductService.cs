
using Core.Application.ViewModels.Client;
using Core.Application.ViewModels.Product;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {
    }
}
