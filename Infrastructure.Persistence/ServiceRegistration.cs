using Core.Application.Interfaces.Repository;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfras(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context

            if (configuration.GetValue<bool>("InMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("InMemoryDB"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), m => m
                .MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }


            #endregion

            #region Service
            
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IDefaultProductRepository, DefaultProductRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepositoy, CategoryRepository>();
            services.AddTransient<IDashBoardRepository, DashBoardRepository>();
            #endregion

        }

    }
}