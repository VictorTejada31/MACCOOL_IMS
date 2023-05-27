using AutoMapper;
using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.DefaultProducts;
using Core.Domain.Entities;

namespace Core.Application.Services
{
    public class DefaultProductService : GenericService<SaveDefaultProductViewModel, DefaultProductViewModel,DefaultProduct>, IDefaultProductService
    {
        private readonly IDefaultProductRepository _repository;
        private readonly IMapper _mapper;
        public DefaultProductService(IDefaultProductRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
