using Core.Application.Interfaces.Services;
using Infrastructure.Identity.Context;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfras(this IServiceCollection services, IConfiguration configuration)
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

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddCookie().AddGoogle(options =>
            {
                options.ClientId = "97044671162-rmebn860hohtr133vifs3bn1v1smu8h4.apps.googleusercontent.com";
                options.ClientSecret = "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";
                
            });

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