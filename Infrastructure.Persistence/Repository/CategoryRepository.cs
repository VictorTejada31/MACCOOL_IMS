using Core.Application.Interfaces.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepositoy
    {
        private ApplicationContext _dbContext;
        public CategoryRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
