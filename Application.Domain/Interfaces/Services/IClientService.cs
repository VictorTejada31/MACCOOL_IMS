using Core.Application.ViewModels.Client;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Services
{
    public interface IClientService : IGenericService<SaveClientViewModel,ClientViewModel,Client>
    {
    }
}
