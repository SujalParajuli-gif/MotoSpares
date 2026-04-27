using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

csharpusing Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoSpares.Application.Configurations;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

        // ── Each person uncomments their own lines below ──
        // Injamamul
        // services.AddScoped<IFinancialReportService, FinancialReportService>();
        // services.AddScoped<ICustomerDetailService, CustomerDetailService>();
        // services.AddScoped<IInvoiceEmailService, InvoiceEmailService>();
        // services.AddScoped<IEmailService, EmailService>();

        // Sujal
        // services.AddScoped<ISalesService, SalesService>();

        // Swodhin
        // services.AddScoped<IStaffService, StaffService>();
        // services.AddScoped<IVendorService, VendorService>();

        // Nischal
        // services.AddScoped<IPartService, PartService>();

        // Niran
        // services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}
