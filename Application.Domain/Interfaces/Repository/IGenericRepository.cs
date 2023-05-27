namespace Core.Application.Interfaces.Repository
{
    public interface IGenericRepository<Entity>
    {
        Task<Entity> AddAsync(Entity entity);
        Task<Entity> UpdateAsync(Entity entity, int id);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GetByIdAsync(int id);
        Task<List<Entity>> GetAllWithInclude(List<string> properties);
    }
}
