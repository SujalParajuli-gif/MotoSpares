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
        // =========================
        // DbContext
        // =========================
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // =========================
        // Identity
        // =========================
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;

            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddSignInManager();

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IFinanceRepository, FinanceRepository>();

        // Services
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFinanceService, FinanceService>();

        // =========================
        // Repositories
        // =========================
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        // =========================
        // Services
        // =========================
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IAuthService, AuthService>();

        // 🔥 Invoice Service
        services.AddScoped<IInvoiceService, InvoiceService>();

        // =========================
        // Email Configuration
        // =========================
        services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

        services.AddScoped<IEmailService, EmailService>();

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