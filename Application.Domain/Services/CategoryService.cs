using AutoMapper;
using Core.Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Category;
using Core.Domain.Entities;

namespace Core.Application.Services
{
    public class CategoryService : GenericService<SaveCategoryViewModel,CategoryViewModel,Category>, ICategoryService
    {
        private readonly ICategoryRepositoy _repository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepositoy repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
