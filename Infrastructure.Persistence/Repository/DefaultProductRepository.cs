using Core.Application.Interfaces.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class DefaultProductRepository : GenericRepository<DefaultProduct>, IDefaultProductRepository
    {
        private ApplicationContext _dbContext;
        public DefaultProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
