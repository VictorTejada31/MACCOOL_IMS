using AutoMapper;
using Core.Application.Interfaces.Repository;

namespace Core.Application.Services
{
    public class GenericService<SaveViewModel,ViewModel,Entity> 
        where SaveViewModel : class
        where Entity : class
        where ViewModel : class
    {

        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper; 
        
        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SaveViewModel> AddAsync(SaveViewModel saveView)
        {
            Entity entity = _mapper.Map<Entity>(saveView);
            Entity result = await _repository.AddAsync(entity);

            return _mapper.Map<SaveViewModel>(entity);
        }

        public async Task<SaveViewModel> UpdateAsync(SaveViewModel saveView, int Id)
        {
            Entity entity = _mapper.Map<Entity>(saveView);
            Entity result = await _repository.UpdateAsync(entity,Id);

            return _mapper.Map<SaveViewModel>(entity);  
        }

        public async Task DeleteAsync(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(entity);
        }

        public async Task<List<ViewModel>> GetAllAsync()
        {
            return _mapper.Map<List<ViewModel>>(await _repository.GetAllAsync());
        }

        public async Task<ViewModel> GetById(int id)
        {
            return _mapper.Map<ViewModel>(await _repository.GetByIdAsync(id));
        }
    }
}
