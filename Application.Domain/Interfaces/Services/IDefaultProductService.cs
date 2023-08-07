using Core.Application.ViewModels.DefaultProducts;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services
{
    public interface IDefaultProductService : IGenericService<SaveDefaultProductViewModel, DefaultProductViewModel, DefaultProduct>
    {
        Task<List<DefaultProductViewModel>> GetAllWithInclude();
    }
}
