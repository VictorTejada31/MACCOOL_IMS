using Core.Application.ViewModels.DefaultProducts;
namespace Core.Application.Interfaces.Services
{
    internal interface IDefaultProductService : IGenericService<SaveDefaultProductViewModel, DefaultProductViewModel>
    {
    }
}
