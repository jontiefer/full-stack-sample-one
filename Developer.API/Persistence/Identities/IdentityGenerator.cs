using Developer.API.Persistence.Identities.Models;
using Microsoft.AspNetCore.Identity;

namespace Developer.API.Persistence.Identities;

public static class IdentityGenerator
{
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Check if the admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new ApplicationRole { Name = "Admin", Description = "Administrator role" };
                await roleManager.CreateAsync(adminRole);
            }

            // Check if the admin user exists
            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = "admin", Email = "" };
                var result = await userManager.CreateAsync(adminUser, configuration["DefaultAdminUserPassword"]);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to seed Admin user in the database.");

                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding Admin user role.  Error Message: {ex.Message}");

            throw;
        }
    }

    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        await EnsureRoleAsync(roleManager, "Admin", "Administrator role");
        await EnsureRoleAsync(roleManager, "User", "Application User Role");
    }

    private static async Task EnsureRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName, string description)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            var roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName, description));

            if (!roleResult.Succeeded)
            {
                throw new Exception($"Failed to create '{roleName}' role.");
            }
        }
    }
}