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
        public ClientService(IClientRepository repository, IMapper mapper) : base(repository,mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
