using AutoMapper;
using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Client;
using Core.Domain.Entities;


namespace Core.Application.Services
{
    public class ClientService : GenericService<SaveClientViewModel,ClientViewModel,Client>, IClientService
    {
        private readonly IClientRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDashBoardService _boardService;
    
        public ClientService(IClientRepository repository, IMapper mapper, IDashBoardService dashBoardService) : base(repository,mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _boardService = dashBoardService;
        }

        public async Task<List<ClientViewModel>> GetClientByUserId(string userId)
        {
            List<ClientViewModel> clients = _mapper.Map<List<ClientViewModel>>(await _repository.GetAllAsync());
            return clients.Where(c => c.CreatedBy == userId).ToList();
        }

        public async Task ProcessClienttBill(int clientId, int total)
        {
            Client client =  await _repository.GetByIdAsync(clientId);
            client.Owed += total;
            await _repository.UpdateAsync(client,clientId);
            await _boardService.UpdateDashBoard(client.CreatedBy, total);
        }


    }
}
