using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotoSpares.Domain.Entities;

csharpusing Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<Vendor> Vendors => Set<Vendor>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        var roles = new List<IdentityRole<Guid>>
        {
            new() { Id = Guid.Parse("a18be9c0-aa65-4af8-bd17-00bd9344e575"), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "1" },
            new() { Id = Guid.Parse("fbd4c411-4a98-4a46-84a4-4e6f2924e000"), Name = "Staff", NormalizedName = "STAFF", ConcurrencyStamp = "2" },
            new() { Id = Guid.Parse("c3aa5a4d-8d7e-4f4a-9f3d-1d6c8d3e0000"), Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = "3" }
        };
        builder.Entity<IdentityRole<Guid>>().HasData(roles);
    }
}
