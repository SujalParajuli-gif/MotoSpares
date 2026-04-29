using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = serviceProvider.GetRequiredService<ILogger<ApplicationUser>>();

        string[] roles = { "Admin", "Staff", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                logger.LogInformation("Role '{Role}' created.", role);
            }
        }

        var adminEmail = "admin@motospares.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                FullName = "System Admin",
                Email = adminEmail,
                UserName = adminEmail,
                PhoneNumber = "9800000000",
                Address = "Kathmandu, Nepal",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Default admin user created: {Email}", adminEmail);
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                logger.LogError("Failed to create admin user: {Errors}", errors);
            }
        }

        // Seed Staff
        var staffEmail = "staff1@motospares.com";
        var staffUser = await userManager.FindByEmailAsync(staffEmail);

        if (staffUser == null)
        {
            staffUser = new ApplicationUser
            {
                FullName = "System Staff",
                Email = staffEmail,
                UserName = staffEmail,
                PhoneNumber = "9811111111",
                Address = "Lalitpur, Nepal",
                Role = "Staff",
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(staffUser, "Staff@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(staffUser, "Staff");
                logger.LogInformation("Default staff user created: {Email}", staffEmail);
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                logger.LogError("Failed to create staff user: {Errors}", errors);
            }
        }

        // Seed Customer
        var customerEmail = "customer@motospares.com";
        var customerUser = await userManager.FindByEmailAsync(customerEmail);

        if (customerUser == null)
        {
            customerUser = new ApplicationUser
            {
                FullName = "System Customer",
                Email = customerEmail,
                UserName = customerEmail,
                PhoneNumber = "9822222222",
                Address = "Bhaktapur, Nepal",
                Role = "Customer",
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(customerUser, "Customer@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customerUser, "Customer");
                logger.LogInformation("Default customer user created: {Email}", customerEmail);
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                logger.LogError("Failed to create customer user: {Errors}", errors);
            }
        }
    }
}
