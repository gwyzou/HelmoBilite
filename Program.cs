using System.Runtime.InteropServices;
using HelmoBilite.Data;
using HelmoBilite.Data.Seeds;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//ATTENTION : THIS CONNECTION STRING IS USED FOR THE DEMO, IT IS NOT SECURE 
//ATTENTION : THIS CONNECTION STRING COULD NOT WORK ON YOUR COMPUTER, PLEASE CHANGE IT TO
//==> Server=(localdb)\mssqllocaldb;Database=helmo_bilite;Trusted_Connection=True;")

if (builder.Environment.IsDevelopment())
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helmo_bilite;Trusted_Connection=True;"));

    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                @"Server=127.0.0.1,2022;Database=HelmoBilite;User Id=sa;Password=Fr00t_L00pth;TrustServerCertificate=True;"));
    }
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer("Data Source=192.168.128.18;User ID=in19b1246;Password=5133;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));
}

/*builder.Services.AddDefaultIdentity<BasicUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<BasicUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{   
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions()
{
    ServeUnknownFileTypes = true
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BasicUser>>();
    await TruckInitialiser.AddDefaultTruck(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
    await RoleDataInitialiser.AddDefaultRole(roleManager);
    await UserDataInitialiser.AddDefaultUser(userManager, scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());

}


app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();