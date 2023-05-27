using Core.Application.Interfaces.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private ApplicationContext _dbContext;
        public ProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
