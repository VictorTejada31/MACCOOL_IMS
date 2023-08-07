using Core.Application.Interfaces.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class DashBoardRepository : GenericRepository<DashBoard> , IDashBoardRepository
    {
        private  ApplicationContext _applicationContext;
        public DashBoardRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }
    }
}
