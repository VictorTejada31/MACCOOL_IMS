using Core.Application.Interfaces.Services;
using Infrastructure.Identity.Context;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfras(IServiceCollection services, IConfiguration configuration)
        {
            #region Context

            if (configuration.GetValue<bool>("InMemoryDatabase"))
            {
                services.AddDbContext<IdentityContex>(options => options.UseInMemoryDatabase("InMemoryDB"));
            }
            else
            {
                services.AddDbContext<IdentityContex>(options => options
                .UseSqlServer(configuration.GetConnectionString("IdentityConnection"), m => m
                .MigrationsAssembly(typeof(IdentityContex).Assembly.FullName)));
            }

            #endregion
            
            #region Identity

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContex>()
                .AddDefaultTokenProviders();

            services.AddAuthentication();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccessDenied";
                
            });

            #endregion

            #region Service 

            services.AddTransient<IAccountService, AccountService>();

            #endregion
        }
    }
}