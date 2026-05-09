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


using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Application.Services;

using MotoSpares.Infrastructure.Repositories;
using MotoSpares.Infrastructure.Services;

// 🔥 Email settings
using MotoSpares.Application.DTOs.Common;

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

        // Services
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IAuthService, AuthService>();

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

        return services;
    }
}