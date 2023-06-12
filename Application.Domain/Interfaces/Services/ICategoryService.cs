using Core.Application.ViewModels.Category;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services
{
    public interface ICategoryService : IGenericService<SaveCategoryViewModel, CategoryViewModel,Category>
    {

    }
}
