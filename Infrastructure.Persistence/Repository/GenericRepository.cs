using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class GenericRepository<Entity> where Entity : class
    {
        private ApplicationContext _context; 
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Entity> AddAsync(Entity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Entity> UpdateAsync(Entity entity, int id)
        {
            var entry = await _context.Set<Entity>().FindAsync(id);
            _context.Entry<Entity>(entry).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Entity entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Entity>> GetAllAsync()
        {
            return await _context.Set<Entity>().ToListAsync();
        }

        public async Task<Entity> GetByIdAsync(int id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }

        public async Task<List<Entity>> GetAllWithInclude(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();
            foreach(var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }


    }
}
