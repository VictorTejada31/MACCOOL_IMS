
namespace Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel,ViewModel,Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        Task<SaveViewModel> AddAsync(SaveViewModel saveView);
        Task<SaveViewModel> UpdateAsync(SaveViewModel saveView, int Id);
        Task DeleteAsync(int id);
        Task<List<ViewModel>> GetAllAsync();
        Task<ViewModel> GetById(int id);
    }
}
