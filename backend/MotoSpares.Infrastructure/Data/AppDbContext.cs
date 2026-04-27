using Microsoft.EntityFrameworkCore;
using MotoSpares.Domain.Entities;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Vendor> Vendors => Set<Vendor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.FullName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(u => u.PasswordHash)
                  .IsRequired();

            entity.Property(u => u.Phone)
                  .HasMaxLength(20);

            entity.Property(u => u.Role)
                  .HasConversion<string>();

            entity.Property(u => u.CreatedAt)
                  .IsRequired();
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.EmployeeCode)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(s => s.Department)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.JoinedAt)
                  .IsRequired();

            entity.HasOne(s => s.User)
                  .WithOne()
                  .HasForeignKey<Staff>(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.VendorName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(v => v.ContactPerson)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(v => v.Phone)
                  .HasMaxLength(20);

            entity.Property(v => v.Email)
                  .HasMaxLength(100);

            entity.Property(v => v.Address)
                  .HasMaxLength(200);

            entity.Property(v => v.CreatedAt)
                  .IsRequired();
        });

        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        modelBuilder.Entity<User>().HasData(new
        {
            Id = adminId,
            FullName = "Admin",
            Email = "admin@motospares.com",
            PasswordHash = "$2a$11$4Wj2kV8smnCmlkboVz44bON6EwIgupC5O/yB8gq0XhYiw8WpfLr3G",
            Role = UserRole.Admin,
            Phone = "0000000000",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}