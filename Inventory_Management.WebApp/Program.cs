using Core.Application;
using Infrastructure.Persistence;
using Infrastructure.Shared;
using Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Core.Application.Enums;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Seeds;
using Core.Application.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddPersistenceInfras(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedLayerInfras(builder.Configuration);
builder.Services.AddIdentityInfras(builder.Configuration);
builder.Services.AddSession();
builder.Services.AddSignalR();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
   try
    {
        var service = scope.ServiceProvider;
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

        await DefaultRoles.AddSeed(roleManager);
        await DefaultAdmin.AddSeed(userManager);
        await DefaultDeveloper.AddSeed(userManager);

    }

    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.MapHub<CashierHub>("/hubs/cashierHub");

app.Run();
