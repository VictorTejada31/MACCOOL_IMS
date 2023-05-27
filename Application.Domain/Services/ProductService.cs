using AutoMapper;
using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Product;
using Core.Domain.Entities;

namespace Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel,ProductViewModel,Product>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
