using Core.Application.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task AddSeed(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "admin@system.com",
                isOnline = false,
                LastConnection = DateTime.Now,
                UserName = "JohnDoe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Plan = Plans.Premium.ToString()

            };

            await userManager.CreateAsync(admin,"Pass123#");
            await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
        }
    }
}
