
using Core.Application.Interfaces.Services;
using Core.Domain.Settings;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedLayerInfras(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSetting>(configuration.GetSection("MailSetting"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
