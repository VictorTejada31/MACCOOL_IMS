
using Core.Application.ViewModels.Client;
using Core.Application.ViewModels.Product;

namespace Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel>
    {
    }
}
