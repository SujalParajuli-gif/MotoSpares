using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoSpares.Application.DTOs.Common;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Application.Services;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;
using MotoSpares.Infrastructure.Repositories;
using MotoSpares.Infrastructure.Services;

namespace MotoSpares.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Identity
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
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IPartRepository, PartRepository>();
        services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        // =========================
        // Application Services
        // =========================
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IFinanceService, FinanceService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPartService, PartService>();
        services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IVehicleService, VehicleService>();

        // =========================
        // Email / PDF Services
        // =========================
        services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPdfService, PdfService>();

        return services;
    }
}