using Microsoft.AspNetCore.Identity;

namespace HelmoBilite.Data.Seeds;

public class RoleDataInitialiser
{
    public const string Admin = "Admin";
    public const string Driver = "Driver";
    public const string Dispatcher = "Dispatcher";
    public const string Client = "Client";

    public static async Task AddDefaultRole(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(Admin))
        {
            var admin = new IdentityRole { Name = Admin, NormalizedName = Admin.ToLower() };
            await roleManager.CreateAsync(admin);
        }

        if (!await roleManager.RoleExistsAsync(Driver))
        {
            var driver = new IdentityRole { Name = Driver, NormalizedName = Driver.ToLower() };
            await roleManager.CreateAsync(driver);
        }

        if (!await roleManager.RoleExistsAsync(Dispatcher))
        {
            var dispatcher = new IdentityRole { Name = Dispatcher, NormalizedName = Dispatcher.ToLower() };
            await roleManager.CreateAsync(dispatcher);
        }

        if (!await roleManager.RoleExistsAsync(Client))
        {
            var client = new IdentityRole { Name = Client, NormalizedName = Client.ToLower() };
            await roleManager.CreateAsync(client);
        }
    }
}