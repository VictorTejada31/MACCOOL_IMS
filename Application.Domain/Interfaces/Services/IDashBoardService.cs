
using Core.Application.ViewModels.DashBoard;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services
{
    public interface IDashBoardService : IGenericService<SaveDashBoardViewModel, DashBoardViewModel, DashBoard>
    {
        Task UpdateDashBoard(string userId, double price);
        Task<DashBoardViewModel> GetAllAsync();
    }
}
