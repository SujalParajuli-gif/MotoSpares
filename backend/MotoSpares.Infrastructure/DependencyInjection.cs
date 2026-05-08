using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Application.Services;
using MotoSpares.Infrastructure.Repositories;

namespace MotoSpares.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;

            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // Register Repositories
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IPartRepository, PartRepository>();
        services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Register Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IPartService, PartService>();
        services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}
