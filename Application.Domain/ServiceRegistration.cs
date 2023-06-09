using Core.Application.Interfaces.Services;
using Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Domain
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IGenericService<,>), typeof(GenericService<,,>));
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IDefaultProductService, DefaultProductService>();
            services.AddTransient<IUserService, UserService>();

        }
    }
}