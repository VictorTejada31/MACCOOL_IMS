using Core.Application.Interfaces.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private ApplicationContext _dbContext;
        public ClientRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
